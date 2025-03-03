using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.CompleteReservation;

public class CompleteReservationCommandHandler(
    IReservationsRepository reservationsRepository,
    ILogger<CompleteReservationCommandHandler> logger) : ICommandHandler<CompleteReservationCommand,
    OneOf<Success, NotFound, Error<ApplicationException>>>
{
    public async Task<OneOf<Success, NotFound, Error<ApplicationException>>> Handle(CompleteReservationCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await reservationsRepository.FindAsync(ReservationId.Create(command.ReservationId));
            if (reservation is null)
            {
                return new NotFound();
            }

            if (reservation.ReservationStatus is Expired)
            {
                throw new ApplicationException(
                    $"Reservation with given ReservationId {command.ReservationId} has been expired.");
            }

            reservation.Complete();

            return new Success();
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}