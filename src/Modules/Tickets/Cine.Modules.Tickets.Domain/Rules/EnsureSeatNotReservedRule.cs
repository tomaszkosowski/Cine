using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

public class EnsureSeatNotReservedRule(Seat seat) : IBusinessRule
{
    public string Message => "Seat is reserved.";
    
    public bool IsBroken() => seat.Status != SeatStatusType.Available;
}