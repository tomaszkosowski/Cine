using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

public class EnsureSeatNotReservedRule(Seat seat) : IBusinessRule
{
    public string Message => "Seat status must not be Reserved.";

    public bool IsBroken() => seat.Status.Equals(SeatStatusType.Reserved);
}