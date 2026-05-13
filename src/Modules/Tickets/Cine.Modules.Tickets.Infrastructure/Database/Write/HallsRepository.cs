using Cine.Modules.Tickets.Domain;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write;

internal sealed class HallsRepository(WriteContext context) : IHallsRepository
{
    public async Task AddAsync(Hall hall)
        => await context.Halls.AddAsync(hall);
}