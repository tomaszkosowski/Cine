# Domain Overview

## What is Cine?

Cine is a cinema management backend. It handles the full lifecycle of running a movie theater: from adding movies to the catalogue and configuring halls, through scheduling screenings, letting customers reserve seats, all the way to processing payment.

---

## Ubiquitous language

| Term | Definition |
|------|-----------|
| **Movie** | A film title with a genre, duration, release date, cast, and directors |
| **Hall** | A physical screening room inside the cinema, containing a fixed set of seats |
| **Seat** | A numbered position within a hall, identified by row and number |
| **Show** | A single screening of a Movie in a Hall at a specific date and time |
| **Reservation** | A customer's claim on one or more Seats for a specific Show |
| **Ticket** | Confirmation that a Seat is held within a Reservation (the output of a completed Reservation) |
| **Payment** | A financial transaction that covers the cost of a Reservation |
| **Discount** | A reduction applied to a Payment based on time of day, day of week, or group size |
| **Schedule** | The start time + duration window of a Show |

---

## Aggregate map

```
Movies          Theater          Shows            Tickets          Sales
──────          ───────          ─────            ───────          ─────
Movie ◄──┐      Hall ◄──┐        Show             Reservation      Payment
Person   │      Seat    │          ├─ HallId ─────────────►        Discount
         │              │          └─ MovieId ────────────►
         │              │                          ├─ ShowId ──────►
         │              └─────────────────────────── HallId ──────►
         └─────────────────────────────────────────── (read)
```

Each module owns its data. Cross-module references are held as **IDs only** — there is no shared database table or shared domain object across module boundaries.

---

## Cross-module relationships

| Consumer module | Produces / Needs | Source module | How |
|-----------------|-----------------|---------------|-----|
| **Shows** | Hall (read-only replica) | Theater | Local read model, populated via `HallCreatedDomainEvent` |
| **Shows** | Movie (read-only replica) | Movies | Local read model, populated via `MovieCreatedDomainEvent` |
| **Tickets** | Show (read-only replica) | Shows | Local read model, populated via `ShowCreatedDomainEvent` |
| **Tickets** | Hall / Seat data | Theater | HTTP call via Refit `ITheaterApiClient` |
| **Sales** | Reservation data (seat count, reserved-at) | Tickets | HTTP call via Refit `ITicketsApiClient` |
| **Sales** | Reservation confirmed signal | Tickets | Integration event `ReservationConfirmedIntegrationEvent` |
| **Tickets** | Payment confirmed signal | Sales | Integration event `PaymentConfirmedIntegrationEvent` |

---

## End-to-end flow

```
1. Create Movie          [Movies API]
2. Create Hall + Seats   [Theater API]
3. Create Show           [Shows API]   ← validates no schedule overlap
4. Create Reservation    [Tickets API]
5. Add Seats             [Tickets API] ← adjacency rules enforced
6. Confirm Reservation   [Tickets API] ← publishes ReservationConfirmedIntegrationEvent
7. Payment Saga starts   [Sales]       ← triggered by integration event
   7a. CreatePayment consumer  ← fetches reservation, calculates price + discount
   7b. ProcessPayment consumer ← simulates payment gateway
   7c. ConfirmPayment consumer ← publishes PaymentConfirmedIntegrationEvent
8. Reservation completed [Tickets]     ← triggered by PaymentConfirmedIntegrationEvent
   Seats marked Purchased
```

---

## Module documentation

- [Movies](./modules/movies.md)
- [Theater](./modules/theater.md)
- [Shows](./modules/shows.md)
- [Tickets](./modules/tickets.md)
- [Sales](./modules/sales.md)
