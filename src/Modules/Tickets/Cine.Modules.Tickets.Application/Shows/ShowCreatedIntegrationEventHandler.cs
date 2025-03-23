using Cine.Modules.Shows.IntegrationEvents.Shows;
using Cine.Modules.Tickets.Application.Shows.CreateShow;
using Cine.Shared.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Tickets.Application.Shows;

internal sealed class ShowCreatedIntegrationEventHandler(IServiceProvider serviceProvider)
    : IIntegrationEventHandler<ShowCreatedIntegrationEvent>
{
    public async Task HandleAsync(ShowCreatedIntegrationEvent @event)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        
        await sender.Send(new CreateShowCommand(@event.ShowId, @event.HallId, @event.StartAt));
    }
}