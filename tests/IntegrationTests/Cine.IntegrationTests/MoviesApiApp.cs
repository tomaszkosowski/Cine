using FastEndpoints.Testing;
using Testcontainers.MsSql;

namespace Cine.IntegrationTests
{
    public class MoviesApiApp : AppFixture<Program>
    {
        private MsSqlContainer _container = null!;

        protected override async Task PreSetupAsync()
        {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithName("ms-sql-integration-tests")
                .Build();

            await _container.StartAsync();
        }

        protected override async Task TearDownAsync()
        {
            await _container.DisposeAsync();
        }
    }
}
