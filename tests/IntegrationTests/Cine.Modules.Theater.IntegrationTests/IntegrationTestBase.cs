using FastEndpoints.Testing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Theater.IntegrationTests;

public abstract class IntegrationTestBase : TestBase<App>, IDisposable
{
    private readonly IServiceScope _scope;

    protected ISender Sender { get; }

    protected IntegrationTestBase(App app)
    {
        _scope = app.Services.CreateScope();

        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}