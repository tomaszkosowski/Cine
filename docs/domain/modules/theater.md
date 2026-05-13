# Theater Module

> **Responsibility:** Manages the physical infrastructure of the cinema — halls and their seats.

- [Domain overview](../overview.md) | [Architecture overview](../../architecture/overview.md)

---

## Domain concepts

### Hall (Aggregate Root)

A `Hall` is a screening room. It owns a collection of `Seat` entities.

| Property | Type | Description |
|----------|------|-------------|
| `HallId` | `TypedId<HallId>` | Unique identifier |
| `Name` | `string` | Human-readable name (e.g. `"Screen 1"`) |
| `Seats` | `IReadOnlyCollection<Seat>` | All seats in the hall |

**Factory:** `Hall.Create(name, seats)`

A hall publishes `HallCreatedDomainEvent` on creation. The Shows module and Tickets module listen to this event to maintain local hall replicas.

### Seat (Entity / Aggregate Root)

Represents a single physical seat within a hall.

| Property | Type | Description |
|----------|------|-------------|
| `SeatId` | `TypedId<SeatId>` | Unique identifier |
| `HallId` | `TypedId<HallId>` | The hall this seat belongs to |
| `Row` | `string` | Row label (e.g. `"A"`, `"B"`) |
| `Number` | `int` | Seat number within the row |
| `Type` | `SeatType` | `Regular` or `Premium` |

**Factories:**
- `Seat.CreateRegular(hallId, row, number)`
- `Seat.CreatePremium(hallId, row, number)`

### SeatType (Value Object)

Encodes the quality tier of a seat. Stored as a lowercase string.

| Value | Description |
|-------|-------------|
| `regular` | Standard seat |
| `premium` | Upgraded seat with better position or comfort |

---

## Domain events

| Event | Raised when | Consumers |
|-------|------------|-----------|
| `HallCreatedDomainEvent` | A new `Hall` is created | Shows module (local read model), Tickets module (local read model) |

---

## Application operations

| Operation | Type | Description |
|-----------|------|-------------|
| `CreateHallCommand` | Command | Creates a hall and its seats |
| `GetHallQuery` | Query | Returns hall details including seats |

---

## HTTP endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/hall/add` | Create a new hall with seats |
| `GET` | `/halls/{hallId}` | Get a hall by ID |
| `GET` | `/halls/{hallId}/seats` | List all seats in a hall |

The `/halls/{hallId}/seats` endpoint is consumed by the **Tickets** module via Refit when a customer adds a seat to a reservation.

---

## See also

- [Tickets module](./tickets.md) — consumes seat data via HTTP
- [Shows module](./shows.md) — keeps a local hall replica
- [Architecture: Event-driven design](../../architecture/event-driven.md)
