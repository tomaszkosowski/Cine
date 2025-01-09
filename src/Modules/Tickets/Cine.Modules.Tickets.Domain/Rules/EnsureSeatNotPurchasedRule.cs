using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

internal sealed class EnsureSeatNotPurchasedRule(Seat seat) : IBusinessRule
{
    public string Message => "Seat status must not be Purchased.";

    public bool IsBroken() => seat.Status.Equals(SeatStatusType.Purchased);
}