using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

internal sealed class EnsureSeatNotOpenedRule(Seat seat) : IBusinessRule
{
    public bool IsBroken() => seat.Status.Equals(SeatStatusType.Open);

    public string Message => "Seat status must not be Open.";
}