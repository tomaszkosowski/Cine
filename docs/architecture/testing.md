# Testing

> How the three test layers are structured, what each covers, and the helpers available.

- [Back to architecture overview](./overview.md)

---

## Test project layout

```
tests/
├── UnitTests/
│   ├── Modules/
│   │   ├── Movies/
│   │   │   ├── Cine.Modules.Movies.Domain.UnitTests
│   │   │   └── Cine.Modules.Movies.Application.UnitTests
│   │   ├── Shows/
│   │   ├── Tickets/
│   │   └── Sales/
│   └── Shared/
│       └── Cine.Shared.Domain.UnitTests          ← test helpers library
├── IntegrationTests/
│   ├── Cine.IntegrationTests                     ← shared fixtures
│   ├── Cine.Modules.Movies.IntegrationTests
│   ├── Cine.Modules.Shows.IntegrationTests
│   ├── Cine.Modules.Theater.IntegrationTests
│   ├── Cine.Modules.Tickets.IntegrationTests
│   └── Cine.Modules.Sales.IntegrationTests (placeholder)  (placeholder)
└── ArchTests/
    └── Cine.ArchitectureTests
```

---

## Unit tests

Unit tests cover the **Domain** and **Application** layers in isolation. No database or message bus is involved.

### Object factories

Every module's unit test project has a `Factories/` folder with `*ObjectFactory` classes that build valid (and invalid) aggregate instances:

```csharp
// Usage
var movie = MovieObjectFactory.CreateValidObject();
var invalid = () => MovieObjectFactory.CreateInvalidObject(); // passes empty title
```

Using factories keeps test data consistent and removes boilerplate from individual tests.

### Shared test helpers (`Cine.Shared.Domain.UnitTests`)

This project is referenced by all domain unit tests and provides two extension methods:

```csharp
// Assert a domain event was raised
var domainEvent = entity.GetDomainEvent<MovieCreatedDomainEvent>();
domainEvent.Should().NotBeNull();

// Assert a business rule was broken
var action = () => MovieObjectFactory.CreateInvalidObject();
action.AssertBrokenRule<EnsureNotEmptyRule>();
```

`AssertBrokenRule<TRule>` verifies both that a `BusinessRuleValidationException` was thrown **and** that the broken rule is of the expected type.

### Example unit test

```csharp
[Fact]
public void Create_WithValidData_ShouldPublishMovieCreatedDomainEvent()
{
    var movie = MovieObjectFactory.CreateValidObject();

    var domainEvent = movie.GetDomainEvent<MovieCreatedDomainEvent>();

    domainEvent.Should().NotBeNull();
    domainEvent?.MovieId.Should().Be(movie.MovieId);
}
```

---

## Integration tests

Integration tests exercise the full stack — HTTP → Application → Infrastructure → real SQL Server and RabbitMQ — using **Testcontainers** and **FastEndpoints.Testing**.

### App fixture

Each module has an `App : AppFixture<Program>` class that:

1. Starts a SQL Server 2022 container (`MsSqlBuilder`) and a RabbitMQ container (`RabbitMqBuilder`) before tests run
2. Injects the container connection strings via `IHostBuilder.ConfigureHostConfiguration`
3. Starts the real ASP.NET Core host against those containers

```csharp
public class App : AppFixture<Api.Program>
{
    private MsSqlContainer _mssql;
    private RabbitMqContainer _rabbitmq;

    protected override async ValueTask PreSetupAsync()
    {
        _mssql = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();
        _rabbitmq = new RabbitMqBuilder("rabbitmq:3-management-alpine").Build();
        await Task.WhenAll(_mssql.StartAsync(), _rabbitmq.StartAsync());
    }
    // ...
}
```

### IntegrationTestBase

Test classes inherit `IntegrationTestBase`, which resolves a scoped `ISender` (MediatR) so tests can send commands directly without going through HTTP:

```csharp
public abstract class IntegrationTestBase : TestBase<App>, IDisposable
{
    protected ISender Sender { get; }
}
```

### Sharing the App fixture

Use xUnit's `[Collection]` / `TestCollection<App>` pattern so multiple test classes share a single container instance (avoiding repeated startup cost):

```csharp
[CollectionDefinition(nameof(CreateMovieTests))]
public class CreateMovieTestCollection : TestCollection<App>;

[Collection(nameof(CreateMovieTests))]
public class CreateMovieTests(App app) : IntegrationTestBase(app) { ... }
```

### Example integration test

```csharp
[Fact]
public async Task CreateMovie_WhenValidCommand_ShouldReturnMovieId()
{
    var command = new CreateMovieCommand("Movie 43", ...);

    var result = await Sender.Send(command, TestContext.Current.CancellationToken);

    result.IsT0.Should().BeTrue();        // success branch
    result.AsT0.Should().NotBeEmpty();    // non-empty Guid
}
```

---

## Architecture tests

`tests/ArchTests/Cine.ArchitectureTests` uses **NetArchTest.Rules** to enforce the layer dependency rules described in [Architecture overview](./overview.md#layer-dependency-rules).

The tests load compiled assemblies listed in `projects.json` and assert:

| Test | Assertion |
|------|-----------|
| `DomainLayer_ShouldNotDependOnApplicationOrInfrastructure` | No type in a `*.Domain` assembly references `*.Application` or `*.Infrastructure` |
| `ApplicationLayer_ShouldNotDependOnInfrastructure` | No type in `*.Application` references `*.Infrastructure` |
| `InfrastructureLayer_ShouldNotDependOnApi` | No type in `*.Infrastructure` references `*.Api` |

Run these whenever adding a new project reference to catch accidental violations early.

---

## Running tests

```bash
# All tests
dotnet test ./Cine.sln

# Single test project
dotnet test ./tests/UnitTests/Modules/Movies/Cine.Modules.Movies.Domain.UnitTests

# Filter by name
dotnet test ./Cine.sln --filter "FullyQualifiedName~CreateMovie"

# Architecture tests only
dotnet test ./tests/ArchTests/Cine.ArchitectureTests
```

---

## See also

- [Architecture overview](./overview.md)
- [Shared Kernel](./shared-kernel.md) — `BusinessRuleValidationException` and domain event helpers
