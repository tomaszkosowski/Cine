using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.AddSeatToReservation;

internal sealed class
    AddSeatToReservationCommandHandler(
        IReservationsRepository reservationsRepository,
        ISeatsRepository seatsRepository,
        ILogger<AddSeatToReservationCommandHandler> logger)
    : ICommandHandler<AddSeatToReservationCommand,
        OneOf<Success, Error<ApplicationException>>>
{
    public async Task<OneOf<Success, Error<ApplicationException>>> Handle(AddSeatToReservationCommand command,
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

            if (reservation.ReservationStatus is Confirmed or Paid or Expired)
            {
                throw new ApplicationException(
                    $"Reservation with given ReservationId {command.ReservationId} has been {reservation.ReservationStatus.GetType().Name}");
            }

            var seatId = SeatId.Create(command.SeatId);

            var seat = await seatsRepository.FindAsync(seatId, reservation.ShowId);
            return seat switch
            {
                null => throw new ApplicationException(
                    $"Seat with given SeatId {seatId} and ShowId {reservation.ShowId} was not found"),
                not null => AddSeatToReservation(reservation, seat)
            };
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }

    private static Success AddSeatToReservation(Reservation reservation, Seat seat)
    {
        reservation.AddSeat(seat);

        return new Success();
    }
}