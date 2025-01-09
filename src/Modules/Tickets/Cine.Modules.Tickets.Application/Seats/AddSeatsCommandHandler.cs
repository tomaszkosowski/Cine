using System.Collections.Immutable;
using Cine.Modules.Tickets.Application.ApiClients.Theater;
using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Seats;

public class
    AddSeatsCommandHandler(
        ISeatsRepository seatsRepository,
        ITheaterApiClient theaterApiClient,
        ILogger<AddSeatsCommandHandler> logger)
    : ICommandHandler<AddSeatsCommand, OneOf<IReadOnlyList<Guid>, Error<ApplicationException>>>
{
    public async Task<OneOf<IReadOnlyList<Guid>, Error<ApplicationException>>> Handle(AddSeatsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var seatDtos = await theaterApiClient.GetSeatsAsync(request.HallId);
            var seats = CreateSeats(seatDtos, request.ShowId).ToList();

            await seatsRepository.AddRangeAsync(seats);

            return seats.Select(seat => (Guid)seat.SeatId).ToImmutableList();
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }

    private static IEnumerable<Seat> CreateSeats(SeatsDto seatsDto, Guid showId)
    {
        return seatsDto.Seats.Select(seatDto => Seat.Create(SeatId.Create(seatDto.SeatId), ShowId.Create(showId), seatDto.Row, seatDto.Number));
    }
}