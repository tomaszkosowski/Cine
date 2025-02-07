using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.RemoveSeatFromReservation;

public class RemoveSeatFromReservationCommandHandler(
    IReservationsRepository reservationsRepository,
    ISeatsRepository seatsRepository,
    ILogger<RemoveSeatFromReservationCommandHandler> logger)
    : ICommandHandler<RemoveSeatFromReservationCommand, OneOf<Success, Error<ApplicationException>>>
{
    public async Task<OneOf<Success, Error<ApplicationException>>> Handle(RemoveSeatFromReservationCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await reservationsRepository.FindAsync(ReservationId.Create(command.ReservationId));
            if (reservation is null)
            {
                throw new ApplicationException(
                    $"Reservation with given ReservationId {command.ReservationId} was not found");
            }
            
            if (reservation.ReservationStatus is Confirmed or Paid)
            {
                throw new ApplicationException(
                    $"Reservation with given ReservationId {command.ReservationId} has been {reservation.ReservationStatus.GetType().Name}");            }

            var seatId = SeatId.Create(command.SeatId);

            var seat = await seatsRepository.FindAsync(seatId, reservation.ShowId);
            return seat switch
            {
                null => throw new ApplicationException($"Seat with given SeatId {command.SeatId} was not found"),
                not null => RemoveSeatFromReservation(reservation, seat)
            };
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }

    private static Success RemoveSeatFromReservation(Reservation reservation, Seat seat)
    {
        reservation.RemoveSeat(seat);

        return new Success();
    }
}