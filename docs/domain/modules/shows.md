# Shows Module

> **Responsibility:** Schedules movies in halls, enforcing that no two shows overlap in the same hall.

- [Domain overview](../overview.md) | [Architecture overview](../../architecture/overview.md)

---

## Domain concepts

### Show (Aggregate Root)

A `Show` is a single screening of a `Movie` in a `Hall` at a defined time window.

| Property | Type | Description |
|----------|------|-------------|
| `ShowId` | `TypedId<ShowId>` | Unique identifier |
| `HallId` | `HallId` | The hall where the screening takes place |
| `MovieId` | `MovieId` | The movie being screened |
| `ScheduledAt` | `Schedule` | Start time and duration |

**Factory:** `Show.Create(hallId, movieId, scheduledAt, otherShows)`

The `otherShows` parameter (a list of `ShowInfo` value objects) is loaded before calling `Create` so that the aggregate can validate the schedule before persisting.

### Schedule (Value Object)

Represents a continuous time window.

| Property | Type | Description |
|----------|------|-------------|
| `StartAt` | `DateTime` | Show start (must not be in the past) |
| `Duration` | `TimeSpan` | Length (must be positive and non-zero) |
| `EndAt` | `DateTime` | Computed: `StartAt + Duration` |

`Schedule.IsOverlapping(other)` returns `true` when two windows share any time: `StartAt < other.EndAt && other.StartAt < EndAt`.

### ShowInfo (Value Object)

A lightweight read-only projection used during schedule validation. Contains only `HallId` and `ScheduledAt` for all existing shows, avoiding the need to load full aggregates.

### Local read models

The Shows module keeps **local copies** of data it needs from other modules:

| Local entity | Source module | Populated via |
|-------------|--------------|---------------|
| `Hall` | Theater | `HallCreatedDomainEvent` |
| `Movie` | Movies | `MovieCreatedDomainEvent` |

These copies hold only the IDs needed to associate a show — they are not full replicas of the originals.

---

## Domain events

| Event | Raised when | Consumers |
|-------|------------|-----------|
| `ShowCreatedDomainEvent` | A new `Show` is created | Tickets module (local read model) |

---

## Business rules

| Rule | Condition |
|------|-----------|
| `EnsureNotOverlapsOtherShows` | A new show's `Schedule` must not overlap any existing show in the same hall |
| `EnsureNotPastRule` on `Schedule.StartAt` | The show cannot be scheduled in the past |
| `EnsureNotZeroRule` on `Schedule.Duration` | Duration must be greater than zero |
| `EnsureNotNegativeRule` on `Schedule.Duration` | Duration must be positive |

The overlap check operates at the aggregate level: it is evaluated inside the `Show` constructor before the ID is assigned, ensuring that an invalid show can never be created.

---

## Application operations

| Operation | Type | Description |
|-----------|------|-------------|
| `CreateShowCommand` | Command | Creates a show after loading all existing shows for the hall |
| `GetShowsQuery` | Query | Returns shows, optionally filtered |

---

## HTTP endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/show/add` | Schedule a new show |
| `GET` | `/shows` | List shows |

---

## See also

- [Theater module](./theater.md) — source of hall data
- [Movies module](./movies.md) — source of movie data
- [Tickets module](./tickets.md) — consumes show data
- [Architecture: Event-driven design](../../architecture/event-driven.md)
