using Cine.Modules.Shows.Domain;

namespace Cine.Modules.Shows.Infrastructure.Database.Write;

internal sealed class HallsRepository(WriteContext context) : IHallsRepository
{
    public async Task AddAsync(Hall hall)
    {
        await context.Halls.AddAsync(hall);
    }
}