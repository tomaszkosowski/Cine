using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.ConfirmReservation;

internal sealed class ConfirmReservationCommandHandler(
    IReservationsRepository reservationsRepository,
    ILogger<ConfirmReservationCommandHandler> logger)
    : ICommandHandler<ConfirmReservationCommand,
        OneOf<Success, NotFound, Error<ApplicationException>>>
{
    public async Task<OneOf<Success, NotFound, Error<ApplicationException>>> Handle(ConfirmReservationCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await reservationsRepository.FindAsync(ReservationId.Create(command.ReservationId));
            if (reservation is null)
            {
                return new NotFound();
            }

            if (reservation.ReservationStatus is Completed or Expired)
            {
                throw new ApplicationException(
                    $"Reservation with given ReservationId {command.ReservationId} has been paid or expired");
            }

            reservation.Confirm();

            return new Success();
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}