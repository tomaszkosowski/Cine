# Cine – Copilot Instructions

## Build, test & lint

```bash
# Restore and build
dotnet restore ./Cine.sln
dotnet build ./Cine.sln --configuration Release --no-restore

# Run all tests
dotnet test ./Cine.sln --configuration Release --no-build --verbosity normal

# Run a single test project
dotnet test ./tests/UnitTests/Modules/Movies/Cine.Modules.Movies.Domain.UnitTests --verbosity normal

# Run a single test by name
dotnet test ./Cine.sln --filter "FullyQualifiedName~CreateMovie_WhenValidCommand"
```

The solution targets **net10.0**. Package versions are managed centrally in `Directory.Packages.props` — never add a `Version` attribute to a `<PackageReference>` element.

## Architecture

The solution is a modular monolith / set of independent API services for a cinema system. Modules: **Movies**, **Sales**, **Shows**, **Theater**, **Tickets**.

Every module follows a strict 4-layer structure:

```
Cine.Modules.<Module>.Domain          ← Entities, business rules, domain events, repo interfaces
Cine.Modules.<Module>.Application     ← CQRS handlers, validators, integration event handlers
Cine.Modules.<Module>.Infrastructure  ← EF Core, repositories, migrations, outbox, RabbitMQ
Cine.Modules.<Module>.Api             ← FastEndpoints HTTP layer, Program.cs
Cine.Modules.<Module>.IntegrationEvents ← Shared event contracts (consumed by other modules)
```

Shared building blocks live in `src/Shared/`:
- `Cine.Shared.Domain` – `Entity`, `ValueObject`, `TypedId<T>`, `IAggregateRoot`, `IBusinessRule`
- `Cine.Shared.Application` – CQRS interfaces, outbox interface, logging helpers, `OneOfFactory`
- `Cine.Shared.Infrastructure` – `UnitOfWork`, domain-events dispatcher, RabbitMQ bus, Hangfire jobs

**Enforced dependency rule** (verified by architecture tests):  
`Domain` ← `Application` ← `Infrastructure` ← `Api`  
Domain must not reference Application or Infrastructure; Application must not reference Infrastructure.

## Key conventions

### Typed IDs
Every aggregate root has a strongly-typed ID using `TypedId<T>`:
```csharp
public record MovieId : TypedId<MovieId>;
```
Use `MovieId.Create()` to generate a new ID; `TypedId<T>.Create(guid)` to wrap an existing one. Typed IDs implicitly convert to `Guid`.

### Aggregates & entities
- Aggregates inherit `Entity` and implement `IAggregateRoot` (marker interface).
- Constructors are **private**; expose a static `Create(...)` factory method.
- Include a **parameterless private constructor** for ORM hydration: `private Movie() { // Blank for ORM. }`
- Raise domain events inside the constructor/mutators using `AddDomainEvent(new SomeDomainEvent(...))`.

### Business rules
Validate invariants with `CheckRule(new EnsureNotEmptyRule(value, nameof(value)))` (inside `Entity` or `ValueObject`).  
Breaking a rule throws `BusinessRuleValidationException`. Available built-in rules: `EnsureNotEmptyRule`, `EnsureNotEmptyCollectionRule`, `EnsureNotNegativeRule`, `EnsureNotPastRule`, `EnsureNotZeroRule`.  
Add new rules in `Cine.Shared.Domain/Rules/` implementing `IBusinessRule`.

### CQRS (MediatR)
- Commands implement `Command<TResult>` (which implements `ICommand<TResult>` → `IRequest<TResult>`).
- Return type is always `OneOf<TSuccess, Error<ApplicationException>>`.
- Handlers are `internal sealed` and implement `ICommandHandler<TCommand, TResult>`.
- Each command gets a companion `*CommandValidator : AbstractValidator<TCommand>` using FluentValidation.
- Queries implement `Query<TResult>` / `IQuery<TResult>`; handlers implement `IQueryHandler<TQuery, TResult>`.

### HTTP layer (FastEndpoints)
Endpoints inherit `Endpoint<TRequest, TResponse>` and are `internal sealed`. Route convention: `Post("resource/verb")` (e.g., `Post("movie/add")`).  
Map between endpoint request/response records and application commands inside `HandleAsync`. Match `OneOf` results with `.Match(...)`.

### Outbox & event pipeline
Domain events → stored as `OutboxMessage` (JSON via Newtonsoft) → processed by `ProcessOutboxJob` (Hangfire, runs every second) → published to RabbitMQ.  
When handling a `DomainEvent` in the Application layer, publish a `*Notification` wrapping it via `IPublisher`. The notification handler maps it to an integration event and hands it to the outbox.

### Unit of work
`UnitOfWorkCommandHandlerDecorator<,>` is registered via Scrutor to wrap **all** `IRequestHandler<,>` implementations. You do not need to call `SaveChanges` manually in handlers.

### Testing conventions
**Unit tests** – Domain objects are built via `*ObjectFactory` classes in a `Factories/` folder.  
Use helpers from `Cine.Shared.Domain.UnitTests`:
- `entity.GetDomainEvent<TDomainEvent>()` – asserts event was raised
- `action.AssertBrokenRule<TRule>()` – asserts `BusinessRuleValidationException` with the right rule type

**Integration tests** – Each module has an `App : AppFixture<Program>` that spins up **Testcontainers** (SQL Server 2022 + RabbitMQ). Test classes inherit `IntegrationTestBase` and use `Sender` (MediatR `ISender`) to exercise the full stack.  
Group related tests with `[Collection]` / `TestCollection<App>` to share a single container instance.

**Architecture tests** – `tests/ArchTests/Cine.ArchitectureTests` uses NetArchTest to enforce layer dependency rules. Run these whenever you add a new inter-project reference.
