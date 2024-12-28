using FastEndpoints.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;

namespace Cine.IntegrationTests;

public abstract class ApiApp(string name) : AppFixture<Modules.Movies.Api.Program>
{
    private MsSqlContainer _container = default!;

    protected override async Task PreSetupAsync()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithName($"ms-sql-{name}-integration-tests")
            .Build();

        await _container.StartAsync();
    }

    protected override IHost ConfigureAppHost(IHostBuilder a)
    {
        a.ConfigureHostConfiguration(b =>
            b.AddInMemoryCollection(new Dictionary<string, string?> {
                { "Database:MsSql:ConnectionString", _container.GetConnectionString() }
            }));

        return base.ConfigureAppHost(a);
    }

    protected override async Task TearDownAsync()
    {
        await _container.DisposeAsync();
    }
}