using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

public class EnsureSeatNotSeparatedRule(Seat seat, IReadOnlyList<Seat> other) : IBusinessRule
{
    public string Message => "Seat must be adjacent to other seats.";

    public bool IsBroken() => NoOtherSeatsAreReserved()
        ? false
        : HasTheSameRow()
            ? !other.Any(seat.IsAdjacent)
            : false;

    private bool NoOtherSeatsAreReserved() => other is { Count: 0 };

    private bool HasTheSameRow() => other.Any(otherSeat => seat.Row == otherSeat.Row);
}