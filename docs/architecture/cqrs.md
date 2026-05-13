# CQRS

> How commands and queries are structured, validated, dispatched, and how results are returned.

- [Back to architecture overview](./overview.md)

---

## Overview

Cine uses **MediatR** as the in-process bus. All writes go through **Commands** and all reads go through **Queries**. Both implement MediatR's `IRequest<TResult>` so they are dispatched with `ISender.Send(...)`.

---

## Commands

### Defining a command

Commands inherit the `Command<TResult>` base record:

```csharp
public record CreateMovieCommand(
    string Title,
    string Description,
    ...
) : Command<OneOf<Guid, Error<ApplicationException>>>;
```

`Command<TResult>` automatically assigns a unique `Id` (`Guid.NewGuid()`), which is part of the `ICommand<TResult>` contract.

### Handling a command

```csharp
internal sealed class CreateMovieCommandHandler(IMoviesRepository repo, ...)
    : ICommandHandler<CreateMovieCommand, OneOf<Guid, Error<ApplicationException>>>
{
    public async Task<OneOf<Guid, Error<ApplicationException>>> Handle(
        CreateMovieCommand request, CancellationToken ct)
    {
        try
        {
            // domain work ...
            return movieId;
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}
```

Handlers are always `internal sealed`. The Unit of Work decorator (registered by Scrutor) calls `SaveChanges` after the handler returns — **handlers never call `SaveChanges` themselves**.

### Validation

Each command has a companion `*CommandValidator : AbstractValidator<TCommand>` (FluentValidation). Validators are registered via `services.AddValidatorsFromAssemblyContaining<IApplicationAssembly>()` and run automatically before the handler.

### Result type

All commands return `OneOf<TSuccess, Error<ApplicationException>>` (or a wider union for operations that can also return `NotFound`). This forces callers to explicitly handle both success and failure paths:

```csharp
var result = await sender.Send(command, ct);
await result.Match(
    async id  => await Send.CreatedAtAsync<GetEndpoint>(...),
    error     => throw error.Value);
```

---

## Queries

### Defining a query

```csharp
public record GetMovieQuery(Guid MovieId) : IQuery<MovieDto?>;
```

### Handling a query

```csharp
internal sealed class GetMovieQueryHandler(IDbConnection db)
    : IQueryHandler<GetMovieQuery, MovieDto?>
{
    public async Task<MovieDto?> Handle(GetMovieQuery request, CancellationToken ct)
        => await db.QueryFirstOrDefaultAsync<MovieDto>(...);
}
```

Query handlers use **Dapper** directly against the read database for performance. They bypass EF Core and the Unit of Work.

---

## HTTP → Application mapping

Endpoints map HTTP request records to application commands/queries and back:

```csharp
internal sealed class AddEndpoint(ISender sender) : Endpoint<Request, Response>
{
    public override void Configure() => Post("movie/add");

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var oneOf = await sender.Send(new CreateMovieCommand(...), ct);
        await oneOf.Match(
            async id    => await Send.CreatedAtAsync<GetEndpoint>(...),
            error       => throw error.Value);
    }
}
```

Endpoints are `internal sealed` and registered automatically by FastEndpoints.

---

## See also

- [Shared Kernel](./shared-kernel.md) — `ICommand`, `IQuery`, `OneOfFactory`
- [Event-driven design](./event-driven.md) — what happens after `SaveChanges`
- [Architecture overview](./overview.md)
