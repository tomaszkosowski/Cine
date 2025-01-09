using Cine.Modules.Tickets.Domain;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write;

internal sealed class SeatsRepository(WriteContext context) : ISeatsRepository
{
    public async Task<Seat?> FindAsync(SeatId seatId, ShowId showId)
    {
        return await context.Seats.FindAsync(seatId, showId);
    }

    public async Task AddRangeAsync(IEnumerable<Seat> seats)
    {
        await context.Seats.AddRangeAsync(seats);
    }
}