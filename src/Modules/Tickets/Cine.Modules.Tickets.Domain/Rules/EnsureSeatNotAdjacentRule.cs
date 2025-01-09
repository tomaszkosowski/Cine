using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

internal sealed class EnsureSeatNotAdjacentRule(Seat seat, IReadOnlyList<Seat> other) : IBusinessRule
{
    public string Message => "Any seat must not left non adjacent.";

    public bool IsBroken() => IsAdjacent();

    private bool IsAdjacent()
    {
        var remainingSeats = other.Where(otherSeat => otherSeat.Row == seat.Row && otherSeat != seat).ToList();
        if (remainingSeats is { Count: <= 1 })
        {
            return false;
        }

        var results = remainingSeats.Zip(remainingSeats.Skip(1), (seatL, seatR)
            => new { SeatL = seatL, SeatR = seatR, IsAdjacent = seatL.IsAdjacent(seatR) });

        return results.Any(result => !result.IsAdjacent);
    }
}