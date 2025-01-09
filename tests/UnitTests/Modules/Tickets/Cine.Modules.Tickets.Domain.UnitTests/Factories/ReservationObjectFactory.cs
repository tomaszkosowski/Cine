namespace Cine.Modules.Tickets.Domain.UnitTests.Factories;

internal static class ReservationObjectFactory
{
    public static Reservation CreateValidObject() => Reservation.Create(ShowId.Create());
}