using Cine.Modules.Shows.Domain.Events;
using MediatR;

namespace Cine.Modules.Shows.Application.Shows.ShowCreated;

public class ShowCreatedDomainEventHandler(IPublisher publisher) : INotificationHandler<ShowCreatedDomainEvent>
{
    public async Task Handle(ShowCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publisher.Publish(new ShowCreatedNotification(notification.EventId, notification), cancellationToken);
    }
}