namespace Cine.Modules.Tickets.Domain;

public interface ISeatsRepository
{
    Task<Seat?> FindAsync(SeatId seatId, ShowId showId);
    Task AddRangeAsync(IEnumerable<Seat> seats);
}