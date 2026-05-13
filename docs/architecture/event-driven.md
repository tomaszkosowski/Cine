# Event-Driven Design

> How domain events become integration events, travel through the outbox, and are consumed by other modules.

- [Back to architecture overview](./overview.md)

---

## Overview

Events flow through three stages:

```
Domain Event
    │
    ▼  (MediatR INotificationHandler)
Domain Event Notification
    │
    ▼  (INotificationHandler publishes to IEventsBus)
Outbox Message  ──►  Hangfire job  ──►  RabbitMQ  ──►  Integration Event Handler
```

Each stage is described below.

---

## Stage 1 — Domain events

Domain events are raised inside aggregate constructors and mutator methods using `AddDomainEvent(...)`:

```csharp
private Movie(...)
{
    // ... set properties
    AddDomainEvent(new MovieCreatedDomainEvent(MovieId, Title, Duration));
}
```

Domain events implement `IDomainEvent` (which implements `INotification` for MediatR).

Events are held in memory on the entity until the Unit of Work calls `IDomainEventsCollector.GetAllDomainEvents()` after `SaveChanges`.

---

## Stage 2 — Outbox

After `DbContext.SaveChangesAsync`, the `UnitOfWork` calls `IDomainEventsDispatcher.DispatchEventsAsync()`.

The dispatcher:
1. Collects all domain events from tracked aggregates
2. Serialises each event as a JSON `OutboxMessage` (Newtonsoft with `TypeNameHandling.All` to preserve type information)
3. Adds the `OutboxMessage` to the `IOutbox` (backed by the EF Core `WriteContext`)

The outbox messages are saved in the same transaction as the domain aggregate changes, guaranteeing consistency.

---

## Stage 3 — Outbox processor (Hangfire)

A `ProcessOutboxJob` Hangfire recurring job runs every second:

```
RecurringJob.AddOrUpdate<ProcessOutboxJob>(job => job.ExecuteAsync(), "* * * * * *");
```

The job:
1. Reads unprocessed `OutboxMessage` rows
2. Deserialises each message back to its domain event type
3. Uses `IDomainEventsMapper` to find the corresponding `INotificationHandler`
4. Publishes the `DomainEventNotification<TDomainEvent>` via MediatR

`IDomainEventsMapper` is populated at startup by scanning assemblies via `IDomainAssembly` markers — no manual registration needed.

---

## Stage 4 — Notification handlers → Integration events

Each domain event that needs to cross module boundaries has a notification handler that converts it and publishes to RabbitMQ:

```csharp
// Tickets module
internal sealed class ReservationConfirmedNotificationHandler(IEventsBus eventsBus)
    : INotificationHandler<ReservationConfirmedNotification>
{
    public async Task Handle(ReservationConfirmedNotification notification, CancellationToken ct)
    {
        var domainEvent = notification.domainEvent;
        await eventsBus.PublishAsync(
            new ReservationConfirmedIntegrationEvent(domainEvent.ReservationId), ct);
    }
}
```

`IEventsBus` is implemented by `RabbitMqEventsBusBackgroundService`.

---

## Stage 5 — Integration event consumers

Other modules register `IIntegrationEventHandler<TEvent>` implementations that are invoked when a message arrives from RabbitMQ:

```csharp
// Tickets module consuming Sales event
internal sealed class PaymentConfirmedIntegrationEventHandler(IServiceProvider sp)
    : IIntegrationEventHandler<PaymentConfirmedIntegrationEvent>
{
    public async Task HandleAsync(PaymentConfirmedIntegrationEvent @event)
    {
        using var scope = sp.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        await sender.Send(new CompleteReservationCommand(@event.ReservationId));
    }
}
```

A new DI scope is created inside the handler to avoid consuming a `DbContext` that was opened on a different thread.

---

## Integration event contracts

Integration events live in dedicated `*.IntegrationEvents` projects so the publishing and consuming modules can both reference them without depending on each other's internal code.

| Project | Events |
|---------|--------|
| `Cine.Modules.Movies.IntegrationEvents` | `MovieCreatedIntegrationEvent` |
| `Cine.Modules.Tickets.IntegrationEvents` | `ReservationConfirmedIntegrationEvent`, `ReservationCreatedIntegrationEvent` |
| `Cine.Modules.Sales.IntegrationEvents` | `PaymentConfirmedIntegrationEvent` |

---

## Event inventory

| Domain event | Module | Integration event published | Consumed by |
|-------------|--------|-----------------------------|-------------|
| `MovieCreatedDomainEvent` | Movies | `MovieCreatedIntegrationEvent` | Shows (local read model) |
| `HallCreatedDomainEvent` | Theater | — | Shows, Tickets (local read models) |
| `ShowCreatedDomainEvent` | Shows | — | Tickets (local read model) |
| `ReservationConfirmedDomainEvent` | Tickets | `ReservationConfirmedIntegrationEvent` | Sales (starts payment saga) |
| `ReservationCreatedDomainEvent` | Tickets | `ReservationCreatedIntegrationEvent` | — |
| `PaymentConfirmedDomainEvent` | Sales | `PaymentConfirmedIntegrationEvent` | Tickets (completes reservation) |

---

## See also

- [Shared Kernel](./shared-kernel.md) — `IOutbox`, `IDomainEventsDispatcher`, `IEventsBus`
- [Sagas](./sagas.md) — how MassTransit consumers fit into this flow
- [Architecture overview](./overview.md)
