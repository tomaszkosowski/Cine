using Cine.Modules.Shows.Application.Halls.AddHall;
using Cine.Modules.Theater.IntegrationEvents.Halls;
using Cine.Shared.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Shows.Application.Halls;

internal sealed class HallCreatedIntegrationEventHandler(IServiceProvider serviceProvider)
    : IIntegrationEventHandler<HallCreatedIntegrationEvent>
{
    public async Task HandleAsync(HallCreatedIntegrationEvent @event)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await sender.Send(new AddHallCommand(@event.HallId));
    }
}