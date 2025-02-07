using Cine.Modules.Tickets.IntegrationEvents.Reservations;
using Cine.Shared.Infrastructure.Events;
using MediatR;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationConfirmed;

internal sealed class ReservationConfirmedNotificationHandler(IEventsBus eventsBus)
    : INotificationHandler<ReservationConfirmedNotification>
{
    public async Task Handle(ReservationConfirmedNotification notification, CancellationToken cancellationToken)
    {
        await eventsBus.PublishAsync(new ReservationConfirmedIntegrationEvent(), cancellationToken);
    }
}