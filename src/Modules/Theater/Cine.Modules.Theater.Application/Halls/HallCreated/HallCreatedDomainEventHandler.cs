using Cine.Modules.Theater.Domain.Events;
using MediatR;

namespace Cine.Modules.Theater.Application.Halls.HallCreated;

public class HallCreatedDomainEventHandler(IPublisher publisher) : INotificationHandler<HallCreatedDomainEvent>
{
    public async Task Handle(HallCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publisher.Publish(new HallCreatedNotification(notification.EventId, notification), cancellationToken);
    }
}