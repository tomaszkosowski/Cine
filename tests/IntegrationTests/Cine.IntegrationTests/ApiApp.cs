using Cine.Shared.Infrastructure.Events;
using FastEndpoints.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace Cine.IntegrationTests;

public abstract class ApiApp(string name) : AppFixture<Modules.Movies.Api.Program>
{
    private MsSqlContainer _mssql = default!;
    private RabbitMqContainer _rabbitmq = default!;

    protected override async Task PreSetupAsync()
    {
        _mssql = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithName($"ms-sql-{name}-integration-tests")
            .Build();

        _rabbitmq = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management-alpine")
            .WithName($"rabbitmq-{name}-integration-tests")
            .WithPortBinding(5672, true)
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();

        await _mssql.StartAsync();
        await _rabbitmq.StartAsync();
    }

    protected override IHost ConfigureAppHost(IHostBuilder a)
    {
        a.ConfigureHostConfiguration(b =>
            b.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "EventsBus:RabbitMq:ConnectionString", _rabbitmq.GetConnectionString() },
                { "Database:MsSql:ConnectionString", _mssql.GetConnectionString() }
            }));

        return base.ConfigureAppHost(a);
    }

    protected override async Task TearDownAsync()
    {
        await _mssql.DisposeAsync();
        await _rabbitmq.DisposeAsync();
    }
}