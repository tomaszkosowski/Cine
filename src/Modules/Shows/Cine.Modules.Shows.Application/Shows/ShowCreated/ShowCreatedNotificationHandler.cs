using Cine.Modules.Shows.IntegrationEvents.Shows;
using Cine.Shared.Infrastructure.Events;
using MediatR;

namespace Cine.Modules.Shows.Application.Shows.ShowCreated;

public class ShowCreatedNotificationHandler(IEventsBus eventsBus) : INotificationHandler<ShowCreatedNotification>
{
    public async Task Handle(ShowCreatedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.domainEvent;
        await eventsBus.PublishAsync(new ShowCreatedIntegrationEvent(domainEvent.ShowId, domainEvent.HallId), cancellationToken);
    }
}