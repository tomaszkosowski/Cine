using Cine.Modules.Theater.Domain;

namespace Cine.Modules.Theater.Infrastructure.Database.Write;

internal sealed class SeatsRepository(WriteContext context) : ISeatsRepository
{
    public async Task AddAsync(Seat seat)
    {
        await context.Seats.AddAsync(seat);
    }
}