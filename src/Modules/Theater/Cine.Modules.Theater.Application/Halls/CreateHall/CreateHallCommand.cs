using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Theater.Application.Halls.CreateHall;

public record CreateHallCommand(
    string Name,
    (int seatsRows, int seatsPerRow) Layout,
    IReadOnlyList<(string Row, int Number)> PremiumSeats)
    : Command<OneOf<(Guid HallId, IReadOnlyList<SeatDto> Seats), Error<ApplicationException>>>;