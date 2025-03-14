using Cine.Shared.Infrastructure.Events;
using FastEndpoints.Testing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Shows.IntegrationTests;

public abstract class IntegrationTestBase : TestBase<App>, IDisposable
{
    private readonly IServiceScope _scope;

    protected ISender Sender { get; }
    
    protected IPublisher Publisher { get; }
    
    protected IEventsBus EventsBus { get; }

    protected IntegrationTestBase(App app)
    {
        _scope = app.Services.CreateScope();

        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        Publisher = _scope.ServiceProvider.GetRequiredService<IPublisher>();
        EventsBus = _scope.ServiceProvider.GetRequiredService<IEventsBus>();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}