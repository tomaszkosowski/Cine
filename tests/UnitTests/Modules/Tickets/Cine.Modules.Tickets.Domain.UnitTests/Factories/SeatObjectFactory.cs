namespace Cine.Modules.Tickets.Domain.UnitTests.Factories;

internal static class SeatObjectFactory
{
    public static Seat CreateValidObject()
        => Seat.Create(SeatId.Create(), ShowId.Create(), "I", 1);

    public static Seat CreateValidObject(string row, int number)
        => Seat.Create(SeatId.Create(), ShowId.Create(), row, number);
}