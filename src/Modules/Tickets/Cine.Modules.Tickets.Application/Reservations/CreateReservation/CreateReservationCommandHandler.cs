using Cine.Modules.Tickets.Application.Shows.GetShow;
using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.CreateReservation;

internal sealed class CreateReservationCommandHandler(
    ISender sender,
    IReservationsRepository reservationsRepository,
    ILogger<CreateReservationCommandHandler> logger)
    : ICommandHandler<CreateReservationCommand, OneOf<Guid, Error<ApplicationException>>>
{
    public async Task<OneOf<Guid, Error<ApplicationException>>> Handle(CreateReservationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var oneOf = await sender.Send(new GetShowQuery(request.ShowId), cancellationToken);
            return await oneOf.Match<Task<OneOf<Guid, Error<ApplicationException>>>>(
                async showDto =>
                {
                    var reservation = Reservation.Create();
                    
                    await reservationsRepository.AddAsync(reservation);

                    return (Guid)reservation.ReservationId;
                },
                notFound => throw new ApplicationException($"Show with given ShowId {request.ShowId} not found"),
                error => throw error.Value);
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}