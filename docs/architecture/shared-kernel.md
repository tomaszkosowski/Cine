# Shared Kernel

> The three `Cine.Shared.*` projects provide the building blocks that every module uses. They are the only code that crosses module boundaries.

- [Back to architecture overview](./overview.md)

---

## Cine.Shared.Domain

### Entity

Base class for all domain entities and aggregate roots.

```csharp
public abstract class Entity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    protected void AddDomainEvent(IDomainEvent domainEvent);
    protected static void CheckRule(IBusinessRule rule);
    public void ClearDomainEvents();
}
```

- `AddDomainEvent` collects events raised during a domain operation; they are dispatched after the `SaveChanges` call by the Unit of Work.
- `CheckRule` throws `BusinessRuleValidationException` if `rule.IsBroken()` returns `true`.

### IAggregateRoot

A marker interface. Every aggregate root implements `Entity` **and** `IAggregateRoot`. Infrastructure code uses this marker to locate aggregates for domain-event collection.

### TypedId\<T\>

Strongly-typed wrapper over `Guid` for aggregate IDs.

```csharp
public record TypedId<TTypedId> where TTypedId : TypedId<TTypedId>, new()
{
    public Guid Value { get; set; }
    public static TTypedId Create();           // new random Guid
    public static TTypedId Create(Guid value); // wrap existing Guid
    public static implicit operator Guid(...); // convert to raw Guid
}
```

Usage: declare `public record MovieId : TypedId<MovieId>;` in the Domain project.

### ValueObject

Base record for immutable value types.

```csharp
public abstract record ValueObject
{
    protected static void CheckRule(IBusinessRule rule);
}
```

### IBusinessRule / BusinessRuleValidationException

```csharp
public interface IBusinessRule
{
    string Message { get; }
    bool IsBroken();
}
```

`BusinessRuleValidationException` carries the broken rule so callers can inspect which rule was violated.

Built-in rules in `Cine.Shared.Domain.Rules`:

| Rule | Validates |
|------|-----------|
| `EnsureNotEmptyRule` | String is not null or whitespace |
| `EnsureNotEmptyCollectionRule` | Collection has at least one element |
| `EnsureNotNegativeRule` | Numeric / TimeSpan is ≥ 0 |
| `EnsureNotZeroRule` | Numeric / TimeSpan is > 0 |
| `EnsureNotPastRule` | DateTime is not in the past |

### ISpecification\<T\>

```csharp
public interface ISpecification<T>
{
    bool IsSatisfiedBy(T candidate);
}
```

Used in the Sales discount system. Composed with `AndSpecification` and `NotSpecification` (also in `Cine.Shared.Domain.Specifications`).

### Utc

A thin wrapper that provides `Utc.Now` — centralises `DateTime.UtcNow` for easier testing and consistency.

---

## Cine.Shared.Application

### ICommand / ICommandHandler

```csharp
public interface ICommand<out TResult> : IRequest<TResult> { Guid Id { get; } }
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult> { }
```

Base record `Command<TResult>` generates `Id = Guid.NewGuid()` automatically.

### IQuery / IQueryHandler

```csharp
public interface IQuery<out TResult> : IRequest<TResult> { }
public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult> { }
```

### OneOfFactory

Helper for constructing the error union type used in command results:

```csharp
OneOfFactory.CreateApplicationError(exception)
// returns Error<ApplicationException>
```

### IOutbox / OutboxMessage

```csharp
public interface IOutbox
{
    void Add(OutboxMessage message);
}
```

`OutboxMessage` stores the serialised domain event type name and JSON content. The Infrastructure layer provides the concrete implementation.

### Logger helpers

`LogApplicationError` extension method on `ILogger` — standardises error logging across all command handlers.

---

## Cine.Shared.Infrastructure

### UnitOfWork\<TContext\>

Wraps an EF Core `DbContext`. After `SaveChangesAsync`, it dispatches all collected domain events via `IDomainEventsDispatcher`.

```csharp
public abstract class UnitOfWork<TContext>(TContext context, IDomainEventsDispatcher dispatcher)
    : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken ct);
}
```

### UnitOfWorkCommandHandlerDecorator\<TCommand, TResult\>

Registered via Scrutor to wrap **every** `IRequestHandler<,>`. Calls `IUnitOfWork.CommitAsync` after the handler returns. Command handlers never call `SaveChanges` directly.

### DomainEventsCollector

Scans all tracked EF Core entities that implement `IAggregateRoot`, collects their `DomainEvents`, and clears them.

### DomainEventsDispatcher

Serialises collected domain events as `OutboxMessage` records (Newtonsoft JSON with type info) and hands them to the `IOutbox`. Events are not published directly to MediatR at this point — the outbox job does that asynchronously.

### DomainEventsMapper

Discovers mappings between domain events and integration events by scanning assemblies that implement `IDomainAssembly`. Used by the outbox processor to know which integration event to create for a given domain event.

### RabbitMqEventsBusBackgroundService

A hosted `BackgroundService` that manages a single persistent RabbitMQ connection. Provides `IEventsBus.PublishAsync` for publishing integration events from notification handlers.

---

## See also

- [CQRS](./cqrs.md)
- [Event-driven design](./event-driven.md)
- [Architecture overview](./overview.md)
