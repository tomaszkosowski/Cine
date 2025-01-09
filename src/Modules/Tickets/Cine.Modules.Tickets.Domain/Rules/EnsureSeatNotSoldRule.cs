using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

internal sealed class EnsureSeatNotSoldRule(Seat seat) : IBusinessRule
{
    public string Message => "Seat is sold.";

    public bool IsBroken() => seat.Status == SeatStatusType.Sold;
}