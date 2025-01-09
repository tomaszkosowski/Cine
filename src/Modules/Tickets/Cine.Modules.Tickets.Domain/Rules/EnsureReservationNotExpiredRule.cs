using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain.Rules;

internal sealed class EnsureReservationNotExpiredRule(Reservation reservation) : IBusinessRule
{
    public string Message => "Reservation must not be Expired.";

    public bool IsBroken() => reservation.ReservationStatus is Expired;
}