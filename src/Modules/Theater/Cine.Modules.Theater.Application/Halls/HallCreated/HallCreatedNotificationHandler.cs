using Cine.Modules.Theater.IntegrationEvents.Halls;
using Cine.Shared.Infrastructure.Events;
using MediatR;

namespace Cine.Modules.Theater.Application.Halls.HallCreated;

public class HallCreatedNotificationHandler(IEventsBus eventsBus) : INotificationHandler<HallCreatedNotification>
{
    public async Task Handle(HallCreatedNotification notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        eventsBus.Publish(new HallCreatedIntegrationEvent(notification.DomainEvent.HallId), cancellationToken);
    }
}