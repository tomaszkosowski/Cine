# Movies Module

> **Responsibility:** Maintains the cinema's movie catalogue.

- [Domain overview](../overview.md) | [Architecture overview](../../architecture/overview.md)

---

## Domain concepts

### Movie (Aggregate Root)

A `Movie` is the central aggregate of this module. It represents a film that can be screened in the cinema.

| Property | Type | Description |
|----------|------|-------------|
| `MovieId` | `TypedId<MovieId>` | Unique identifier |
| `Title` | `string` | Film title (required, max 100 chars) |
| `Description` | `string` | Short synopsis (max 500 chars) |
| `MovieGenre` | `MovieGenre` | Genre value object |
| `Duration` | `TimeOnly` | Running time |
| `ReleaseDate` | `DateOnly` | Original release date |
| `Directors` | `IReadOnlyCollection<Person>` | Directing team |
| `Cast` | `IReadOnlyCollection<Person>` | Acting cast |

**Creation rule:** `Title` must not be empty (`EnsureNotEmptyRule`). Violations throw a `BusinessRuleValidationException`.

**Factory:** `Movie.Create(title, description, genre, duration, releaseDate, directors, cast)`

### Person (Entity)

Represents a real-world person (director or actor) associated with movies.

| Property | Type |
|----------|------|
| `PersonId` | `TypedId<PersonId>` |
| `FirstName` | `string` |
| `LastName` | `string` |

People are shared between a movie's `Directors` and `Cast` collections. They are looked up by name from a `IPeopleRepository` before a movie is created — if a named person doesn't exist yet they are created on demand.

### MovieGenre (Value Object)

Wraps a genre string. Used to categorise movies (e.g. `"Comedy"`, `"Drama"`).

---

## Domain events

| Event | Raised when | Consumers |
|-------|------------|-----------|
| `MovieCreatedDomainEvent` | A new `Movie` is created | Shows module (populates local read model) |
| `MovieUpdatedDomainEvent` | Directors or cast are added to an existing `Movie` | — |

---

## Business rules

| Rule | Condition |
|------|-----------|
| `EnsureNotEmptyRule` on `Title` | Title must not be null or whitespace |

---

## Application operations

| Operation | Type | Description |
|-----------|------|-------------|
| `CreateMovieCommand` | Command | Creates a new movie; looks up or creates people for directors/cast |
| `GetMovieQuery` | Query | Returns a movie by ID |

**Command result:** `OneOf<Guid, Error<ApplicationException>>` — returns the new `MovieId` on success.

---

## HTTP endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/movie/add` | Create a new movie |
| `GET` | `/movie/{movieId}` | Get a movie by ID |

---

## Integration events published

| Event | Triggered by |
|-------|-------------|
| `MovieCreatedIntegrationEvent` | `MovieCreatedDomainEvent` via outbox → RabbitMQ |

The Shows module subscribes to `MovieCreatedIntegrationEvent` to keep a local read-only replica of movie data.

---

## See also

- [Architecture: CQRS](../../architecture/cqrs.md)
- [Architecture: Event-driven design](../../architecture/event-driven.md)
