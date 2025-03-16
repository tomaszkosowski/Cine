using Cine.Modules.Tickets.Application.ApiClients.Theater;
using FastEndpoints.Testing;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Tickets.IntegrationTests;

public abstract class IntegrationTestBase : TestBase<App>, IDisposable
{
    private readonly IServiceScope _scope;
    
    protected IConfiguration Configuration { get; }

    protected ISender Sender { get; }
    
    protected ITheaterApiClient TheaterApiClient { get; }

    protected IntegrationTestBase(App app)
    {
        _scope = app.Services.CreateScope();

        Configuration = _scope.ServiceProvider.GetRequiredService<IConfiguration>();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        TheaterApiClient = _scope.ServiceProvider.GetRequiredService<ITheaterApiClient>();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}