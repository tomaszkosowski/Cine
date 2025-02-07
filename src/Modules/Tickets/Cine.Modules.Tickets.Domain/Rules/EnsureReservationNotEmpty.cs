using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

internal sealed class EnsureReservationNotEmpty(Reservation reservation) : IBusinessRule
{
    public string Message => "Reservation must not be empty.";

    public bool IsBroken() => reservation.Seats is { Count: 0 };
}