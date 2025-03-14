using FastEndpoints.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace Cine.Modules.Theater.IntegrationTests;

public class App : AppFixture<Api.Program>
{
    private MsSqlContainer _mssql = default!;
    private RabbitMqContainer _rabbitmq = default!;

    protected override async Task PreSetupAsync()
    {
        _mssql = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithName($"ms-sql-integration-tests-{Guid.NewGuid()}")
            .Build();

        _rabbitmq = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management-alpine")
            .WithName($"rabbitmq-integration-tests-{Guid.NewGuid()}")
            .WithPortBinding(5672, true)
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();

        await Task.WhenAll(_mssql.StartAsync(), _rabbitmq.StartAsync());
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
}