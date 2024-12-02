using Cine.Modules.Tickets.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write;

internal sealed class ShowsRepository(WriteContext context) : IShowsRepository
{
    public async Task AddAsync(Show show)
    {
        await context.Shows.AddAsync(show);
    }

    public async Task<bool> ExistsAsync(ShowId showId)
    {
        return await context.Shows.AnyAsync(show => show.ShowId == showId);
    }
}