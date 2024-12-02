using Cine.Modules.Tickets.Domain.Events;
using MediatR;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationCreated;

public class ReservationCreatedDomainEventHandler(IPublisher publisher)
    : INotificationHandler<ReservationCreatedDomainEvent>
{
    public async Task Handle(ReservationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publisher.Publish(new ReservationCreatedNotification(notification.EventId, notification),
            cancellationToken);
    }
}