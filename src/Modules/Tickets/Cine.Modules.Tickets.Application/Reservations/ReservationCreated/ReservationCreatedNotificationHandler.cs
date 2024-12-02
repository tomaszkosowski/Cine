using Cine.Modules.Tickets.IntegrationEvents.Reservations;
using Cine.Shared.Infrastructure.Events;
using MediatR;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationCreated;

public class ReservationCreatedNotificationHandler(IEventsBus eventsBus)
    : INotificationHandler<ReservationCreatedNotification>
{
    public async Task Handle(ReservationCreatedNotification notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        eventsBus.Publish(new ReservationCreatedIntegrationEvent(), cancellationToken);
    }
}