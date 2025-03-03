using System.Collections.Immutable;
using Cine.Modules.Tickets.Application.Reservations.RemoveSeatFromReservation;
using Cine.Modules.Tickets.Domain;
using Cine.Modules.Tickets.Domain.Events;
using Cine.Shared.Application.Logger;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationExpired;

public class ReservationExpiredDomainEventHandler(
    ISender sender,
    IReservationsRepository reservationsRepository,
    ILogger<ReservationExpiredDomainEventHandler> logger) : INotificationHandler<ReservationExpiredDomainEvent>
{
    public async Task Handle(ReservationExpiredDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await reservationsRepository.FindAsync(ReservationId.Create(notification.ReservationId));
            if (reservation is null)
            {
                throw new ApplicationException(
                    $"Reservation with given ReservationId {notification.ReservationId} was not found");
            }

            if (reservation.ReservationStatus is Confirmed or Completed)
            {
                throw new ApplicationException(
                    $"Reservation with given ReservationId {notification.ReservationId} has been paid");
            }
            
            var seats = reservation.Seats.ToImmutableList();
            foreach (var seat in seats)
            {
                await sender.Send(new RemoveSeatFromReservationCommand(seat.ReservationId, seat.SeatId),
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            throw;
        }
    }
}