using Cine.Modules.Tickets.Domain.Events;
using MediatR;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationConfirmed;

public record ReservationConfirmedDomainEventHandler(IPublisher publisher)
    : INotificationHandler<ReservationConfirmedDomainEvent>
{
    public async Task Handle(ReservationConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publisher.Publish(new ReservationConfirmedNotification(notification.EventId, notification),
            cancellationToken);
    }
}