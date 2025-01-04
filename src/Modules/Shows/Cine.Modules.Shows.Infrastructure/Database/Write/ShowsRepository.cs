using Cine.Modules.Shows.Domain;

namespace Cine.Modules.Shows.Infrastructure.Database.Write;

internal sealed class ShowsRepository(WriteContext context) : IShowsRepository
{
    public async Task AddAsync(Show show)
    {
        await context.Shows.AddAsync(show);
    }
}