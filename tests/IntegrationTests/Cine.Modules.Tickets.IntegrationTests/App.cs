using Cine.Modules.Tickets.Application.ApiClients.Theater;
using FastEndpoints.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace Cine.Modules.Tickets.IntegrationTests;

public class App : AppFixture<Program>
{
    private MsSqlContainer _mssql = default!;
    private RabbitMqContainer _rabbitmq = default!;
    private ITheaterApiClient _theaterApiClient = Substitute.For<ITheaterApiClient>();

    protected override async ValueTask PreSetupAsync()
    {
        _mssql = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .WithName($"ms-sql-integration-tests-{Guid.NewGuid()}")
            .Build();

        _rabbitmq = new RabbitMqBuilder("rabbitmq:3-management-alpine")
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
                { "Database:MsSql:ConnectionString", _mssql.GetConnectionString() },
                { "Features:Reservations:ReservationExpiryTime", "00:15:00"}
            }));

        return base.ConfigureAppHost(a);
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        var descriptor = s.SingleOrDefault(service => service.ServiceType == typeof(ITheaterApiClient));
        if (descriptor is not null)
        {
            s.Remove(descriptor);
        }

        s.AddSingleton(_theaterApiClient);
    }
}