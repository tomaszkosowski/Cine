using Cine.Modules.Theater.Domain;
using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Theater.Application.Halls.CreateHall;

public class CreateHallCommandHandler(IHallsRepository hallsRepository, ISeatsRepository seatsRepository)
    : ICommandHandler<CreateHallCommand,
        OneOf<(Guid HallId, IReadOnlyList<SeatDto> Seats), Error<ApplicationException>>>
{
    private static readonly string[] Numerals =
        ["I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII"];

    public async Task<OneOf<(Guid HallId, IReadOnlyList<SeatDto> Seats), Error<ApplicationException>>> Handle(
        CreateHallCommand request,
        CancellationToken cancellationToken)
    {
        var hall = Hall.Create(request.Name, []);

        var seats = await BuildSeatsFromLayoutAsync(hall.HallId, request.Layout).ToListAsync(cancellationToken);
        hall.AssignSeats(seats.ToList());

        await hallsRepository.AddAsync(hall);

        return ((Guid)hall.HallId,
            hall.Seats.Select(seat => new SeatDto(seat.Row, seat.Number, seat.Type.Value)).ToList());
    }

    private async IAsyncEnumerable<Seat> BuildSeatsFromLayoutAsync(HallId hallId,
        (int seatsRows, int seatsPerRow) layout)
    {
        var (seatsRows, seatsPerRow) = layout;

        foreach (var seatRow in Enumerable.Range(0, seatsRows))
        foreach (var seatNumber in Enumerable.Range(1, seatsPerRow - 1))
        {
            var row = Numerals[seatRow];

            var seat = Seat.CreateRegular(hallId, row, seatNumber);
            await seatsRepository.AddAsync(seat);

            yield return seat;
        }
    }
}