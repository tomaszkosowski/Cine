using Cine.Modules.Tickets.Domain;
using Cine.Modules.Tickets.Domain.Events;
using MediatR;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationCreated;

public class ReservationCreatedDomainEventHandler : INotificationHandler<ReservationCreatedDomainEvent>
{
    public async Task Handle(ReservationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        foreach (var seat in notification.SeatsToBeReserved)
        {
            seat.ChangeStatus(SeatStatusType.Reserved);
        }
    }
}