# Tickets Module

> **Responsibility:** Manages customer seat reservations for shows, including seat selection, reservation lifecycle, and expiry.

- [Domain overview](../overview.md) | [Architecture overview](../../architecture/overview.md)

---

## Domain concepts

### Reservation (Aggregate Root)

A `Reservation` is the central aggregate. It holds a customer's claim on a set of seats for a specific show.

| Property | Type | Description |
|----------|------|-------------|
| `ReservationId` | `TypedId<ReservationId>` | Unique identifier |
| `ShowId` | `ShowId` | The show being reserved |
| `Seats` | `IReadOnlyList<Seat>` | Seats included in this reservation |
| `ReservationStatus` | `IReservationStatus` | Current lifecycle state |

**Factory:** `Reservation.Create(showId)` — starts in `Unpaid` status.

#### Lifecycle transitions

```
         ┌──────────┐
         │  Unpaid  │  ← initial state
         └────┬─────┘
         ┌────┴─────┐  ← Confirm() called (customer clicks "pay")
         │Confirmed │
         └────┬─────┘
         ┌────┴─────┐  ← Complete() called after payment confirmed
         │Completed │  (terminal)
         └──────────┘

         ┌──────────┐
         │  Unpaid  │
         └────┬─────┘
         ┌────┴─────┐  ← Expire() called by background job
         │ Expired  │  (terminal)
         └──────────┘
```

Each status is an **immutable record** (`Unpaid`, `Confirmed`, `Completed`, `Expired`) implementing `IReservationStatus`. Advancing to an invalid state throws `InvalidOperationException`.

Timestamps are carried inside status records:
- `Unpaid` — stores `ReservedAt`
- `Confirmed` — stores `ConfirmedAt` + `ReservedAt`
- `Completed` — stores `PaidAt` + `ReservedAt`
- `Expired` — stores `ExpiredAt` + `ReservedAt`

#### Reservation expiry

A Hangfire recurring job calls `ExpireReservationsCommand` periodically. Any reservation still `Unpaid` after the configured `Features:Reservations:ReservationExpiryTime` is expired automatically.

---

### Seat (Entity)

Within the Tickets module, a `Seat` represents an instance of a theater seat **for a specific show**. It tracks reservation status independently of the Theater module's seat definition.

| Property | Type | Description |
|----------|------|-------------|
| `SeatId` | `SeatId` | Matches the ID from the Theater module |
| `ShowId` | `ShowId` | The show this seat belongs to |
| `Row` | `string` | Row label |
| `Number` | `int` | Seat number |
| `Status` | `SeatStatusType` | `Open` / `Reserved` / `Purchased` |
| `ReservationId` | `ReservationId?` | Set when the seat is reserved |

#### Seat status transitions

| From | To | When |
|------|----|------|
| `Open` | `Reserved` | Seat added to a reservation |
| `Reserved` | `Open` | Seat removed from a reservation |
| `Reserved` | `Purchased` | Reservation completed after payment |

The `Seat.ChangeStatus()` method validates each transition with guard rules before changing state.

---

## Business rules

### Seat selection rules (enforced on `Reservation.AddSeat`)

| Rule | Description |
|------|-------------|
| `EnsureSeatNotSeparatedRule` | When seats are in the same row, a new seat must be adjacent to an already-reserved seat. The first seat in a reservation has no restriction. |

### Seat removal rules (enforced on `Reservation.RemoveSeat`)

| Rule | Description |
|------|-------------|
| `EnsureSeatNotAdjacentRule` | Removing a seat must not leave a gap between remaining seats in the same row (no isolated single-seat holes). |

### Seat status rules

| Rule | Prevents |
|------|---------|
| `EnsureSeatNotReservedRule` | Reserving an already reserved seat |
| `EnsureSeatNotPurchasedRule` | Changing status of a purchased seat |
| `EnsureSeatNotOpenedRule` | Opening a seat that is already open |

### Reservation confirmation rules

| Rule | Description |
|------|-------------|
| `EnsureReservationNotEmpty` | A reservation must have at least one seat before it can be confirmed |

---

## Domain events

| Event | Raised when |
|-------|------------|
| `ReservationCreatedDomainEvent` | New reservation created |
| `ReservationConfirmedDomainEvent` | Reservation confirmed by customer |
| `ReservationExpiredDomainEvent` | Reservation expired by background job |
| `ReservationCompletedDomainEvent` | Reservation completed after payment |
| `SeatReservedDomainEvent` | Seat status changed to `Reserved` |
| `SeatReleasedDomainEvent` | Seat status changed back to `Open` |
| `SeatPurchasedDomainEvent` | Seat status changed to `Purchased` |

---

## Integration events

| Event | Direction | Trigger |
|-------|-----------|---------|
| `ReservationConfirmedIntegrationEvent` | **Published** to RabbitMQ | `ReservationConfirmedDomainEvent` → outbox |
| `ReservationCreatedIntegrationEvent` | **Published** to RabbitMQ | `ReservationCreatedDomainEvent` → outbox |
| `PaymentConfirmedIntegrationEvent` | **Consumed** from RabbitMQ | Triggers `CompleteReservationCommand` |

The Sales module subscribes to `ReservationConfirmedIntegrationEvent` to start the payment saga. When payment finishes, the Sales module publishes `PaymentConfirmedIntegrationEvent` back, which causes the Tickets module to mark the reservation as `Completed` and seats as `Purchased`.

---

## Application operations

| Operation | Type | Description |
|-----------|------|-------------|
| `CreateReservationCommand` | Command | Creates an `Unpaid` reservation for a show |
| `AddSeatToReservationCommand` | Command | Adds a seat; fetches seat data from Theater API |
| `RemoveSeatFromReservationCommand` | Command | Removes a seat from a reservation |
| `ConfirmReservationCommand` | Command | Advances reservation to `Confirmed` |
| `CompleteReservationCommand` | Command | Advances reservation to `Completed`; marks seats `Purchased` |
| `ExpireReservationsCommand` | Command | Batch-expires all overdue `Unpaid` reservations |
| `GetReservationQuery` | Query | Returns reservation details |
| `GetReservationTicketsQuery` | Query | Returns confirmed seat tickets for a reservation |

---

## HTTP endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/reservation/create` | Create a reservation |
| `POST` | `/reservation/{id}/seat/add` | Add a seat |
| `DELETE` | `/reservation/{id}/seat/remove` | Remove a seat |
| `POST` | `/reservation/{id}/confirm` | Confirm (initiates payment flow) |
| `GET` | `/reservations/{id}` | Get reservation (also consumed by Sales via Refit) |
| `GET` | `/reservations/{id}/tickets` | Get tickets for a completed reservation |

---

## See also

- [Shows module](./shows.md) — source of show data
- [Theater module](./theater.md) — source of seat layout (HTTP)
- [Sales module](./sales.md) — consumes reservation confirmation, drives payment
- [Domain overview: End-to-end flow](../overview.md#end-to-end-flow)
- [Architecture: Event-driven design](../../architecture/event-driven.md)
- [Architecture: Sagas](../../architecture/sagas.md)
