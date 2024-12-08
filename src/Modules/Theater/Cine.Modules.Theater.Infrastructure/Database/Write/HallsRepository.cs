using Cine.Modules.Theater.Domain;

namespace Cine.Modules.Theater.Infrastructure.Database.Write;

internal sealed class HallsRepository(WriteContext context) : IHallsRepository
{
    public async Task AddAsync(Hall hall)
    {
        await context.Halls.AddAsync(hall);
    }
}