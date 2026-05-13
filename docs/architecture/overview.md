# Architecture Overview

> Technical structure of the Cine solution — how projects are organized, how layers relate, and how modules are wired together at runtime.

- [Back to docs index](../index.md)

---

## Solution structure

```
Cine.sln
├── src/
│   ├── Modules/
│   │   ├── Movies/
│   │   ├── Shows/
│   │   ├── Theater/
│   │   ├── Tickets/
│   │   └── Sales/
│   └── Shared/
│       ├── Cine.Shared.Domain
│       ├── Cine.Shared.Application
│       └── Cine.Shared.Infrastructure
└── tests/
    ├── UnitTests/
    ├── IntegrationTests/
    └── ArchTests/
```

---

## Module project structure

Every module follows the same 5-project pattern:

```
Cine.Modules.<Module>.Domain
Cine.Modules.<Module>.Application
Cine.Modules.<Module>.Infrastructure
Cine.Modules.<Module>.Api
Cine.Modules.<Module>.IntegrationEvents
```

The **Sales** module has an additional project:

```
Cine.Modules.Sales.Application.Sagas   ← MassTransit state machine + consumers
```

---

## Layer dependency rules

```
Domain  ←  Application  ←  Infrastructure  ←  Api
```

| Layer | May depend on | Must NOT depend on |
|-------|--------------|-------------------|
| `Domain` | `Shared.Domain` only | Application, Infrastructure, Api |
| `Application` | Domain, `Shared.Application`, `Shared.Domain` | Infrastructure, Api |
| `Infrastructure` | Application, Domain, `Shared.Infrastructure` | Api |
| `Api` | Application, Infrastructure | — |

These rules are **enforced by automated architecture tests** in `tests/ArchTests/Cine.ArchitectureTests`. Any new project reference that violates them will fail the build.

---

## Dependency injection wiring

Each module exposes two extension methods:

```csharp
// Application layer
services.AddApplication(opts => opts.MsSqlConnectionString = "...");

// Infrastructure layer  
services.AddInfrastructure(opts => {
    opts.MsSqlConnectionString = "...";
    opts.RabbitMqConnectionString = "...";
});

// Infrastructure middleware
app.UseInfrastructure(); // applies migrations, starts Hangfire, triggers recurring jobs
```

The `Api` project's `Program.cs` calls both, then calls `app.UseFastEndpoints()`.

---

## Shared projects

See [Shared Kernel](./shared-kernel.md) for full details.

| Project | Purpose |
|---------|---------|
| `Cine.Shared.Domain` | Base classes: `Entity`, `ValueObject`, `TypedId<T>`, `IAggregateRoot`, `IBusinessRule`, `ISpecification<T>` |
| `Cine.Shared.Application` | CQRS interfaces, outbox interface, logging helpers, `OneOfFactory` |
| `Cine.Shared.Infrastructure` | `UnitOfWork`, domain-event dispatcher, RabbitMQ bus, Hangfire job base |

---

## Infrastructure per module

Each module's `Infrastructure` project wires up:

| Component | Technology |
|-----------|-----------|
| Write database | EF Core + SQL Server (`WriteContext`) |
| Migrations | EF Core `Database.Migrate()` on startup |
| Unit of Work | `UnitOfWorkCommandHandlerDecorator<,>` via Scrutor |
| Outbox | `OutboxAccessor` backed by EF Core; processed by `ProcessOutboxJob` |
| Background jobs | Hangfire (in-memory storage) |
| Message bus | RabbitMQ via `RabbitMqEventsBusBackgroundService` (MassTransit for Sales) |
| Tracing | OpenTelemetry → Jaeger (OTLP exporter) |

---

## Cross-module communication

Modules communicate in two ways:

| Mechanism | When used | Example |
|-----------|-----------|---------|
| **Integration events via RabbitMQ** | Async, fire-and-forget notifications | `ReservationConfirmedIntegrationEvent` |
| **HTTP via Refit** | Synchronous data queries at request time | Tickets → Theater `/halls/{id}/seats` |

Modules never share a database or call each other's internal code.

---

## See also

- [Shared Kernel](./shared-kernel.md)
- [CQRS](./cqrs.md)
- [Event-driven design](./event-driven.md)
- [Sagas](./sagas.md)
- [Testing](./testing.md)
