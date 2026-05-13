# 🎬 Cine

A learning project exploring how to build a **modular-monolith** backend with Docker, applying **Domain-Driven Design**, **CQRS**, **event-driven messaging**, and related patterns in a realistic cinema-management context.

## Modules

| Module | Port | Responsibility |
|--------|------|----------------|
| **Movies** | 8080 | Movie catalogue – titles, genres, cast & directors |
| **Theater** | 8082 | Screens and seating configuration |
| **Shows** | 8083 | Scheduling movies in theaters |
| **Tickets** | 8081 | Seat reservations |
| **Sales** | 8084 | Payment flow orchestrated via a saga |

Each module is an independently deployable service sharing the same repository, following a clean 4-layer architecture:

```
Domain → Application → Infrastructure → Api
```

## Tech stack

- **.NET 10** · **C#**
- **FastEndpoints** – minimal-API HTTP layer
- **MediatR** – in-process CQRS bus
- **Entity Framework Core** + **SQL Server** – write-side persistence
- **MassTransit** + **RabbitMQ** – integration events & saga orchestration
- **Hangfire** – recurring background jobs (outbox processor)
- **FluentValidation** – command validation
- **OpenTelemetry** → **Jaeger** – distributed tracing
- **xUnit v3** + **Testcontainers** + **FastEndpoints.Testing** – integration tests
- **NetArchTest** – enforced architecture rules

## Running locally

**Prerequisites:** Docker Desktop

```bash
docker compose up --build
```

| Service | URL |
|---------|-----|
| Movies API | http://localhost:8080 |
| Tickets API | http://localhost:8081 |
| Theater API | http://localhost:8082 |
| Shows API | http://localhost:8083 |
| Sales API | http://localhost:8084 |
| RabbitMQ management | http://localhost:15672 (guest / guest) |
| Jaeger UI | http://localhost:16686 |

SQL Server is exposed on port **1433**. EF Core migrations are applied automatically on startup.

## Running tests

```bash
# All tests
dotnet test ./Cine.sln

# Single project
dotnet test ./tests/UnitTests/Modules/Movies/Cine.Modules.Movies.Domain.UnitTests

# Filter by name
dotnet test ./Cine.sln --filter "FullyQualifiedName~CreateMovie"
```

Integration tests spin up real SQL Server and RabbitMQ containers via Testcontainers — no manual setup needed.

## Key concepts explored

### Domain-Driven Design
Aggregates enforce their own invariants through **business rules** (`IBusinessRule` / `CheckRule()`). Every aggregate root raises **domain events** when something meaningful happens, keeping behaviour close to the data it governs.

### Typed IDs
All aggregate identifiers are strongly-typed wrappers over `Guid` (e.g. `MovieId`, `ShowId`) using a generic `TypedId<T>` base record. This prevents accidentally passing the wrong ID type at compile time.

### CQRS
Commands and queries are separate MediatR requests. Commands return a discriminated union `OneOf<TSuccess, Error<ApplicationException>>` so callers are forced to handle both the happy path and errors explicitly — no unchecked exceptions leaking up to the HTTP layer.

### Outbox pattern
Domain events are not published directly to RabbitMQ. Instead they are serialised as `OutboxMessage` rows and flushed by a **Hangfire recurring job** every second. This guarantees at-least-once delivery even if the message broker is temporarily unavailable.

### Saga (Sales module)
The payment flow is a long-running process that reacts to events (`ReservationConfirmed` → `PaymentCreated` → `PaymentSucceeded` / `PaymentFailed`) and transitions through states. Implemented as a **MassTransit state-machine saga** (`PaymentSaga`).

### Unit of Work decorator
A Scrutor-registered `UnitOfWorkCommandHandlerDecorator<,>` wraps every MediatR command handler to commit the EF Core transaction and dispatch domain events automatically — handlers never call `SaveChanges` themselves.

### Architecture tests
`tests/ArchTests` uses **NetArchTest** to assert that layer dependency rules are never broken (e.g. Domain must not reference Application or Infrastructure). These run as part of the normal test suite.
