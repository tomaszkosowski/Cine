using Cine.Modules.Shows.Domain;

namespace Cine.Modules.Shows.Infrastructure.Database.Write;

internal sealed class MoviesRepository(WriteContext context) : IMoviesRepository
{
    public async Task AddAsync(Movie movie)
    {
        await context.Movies.AddAsync(movie);
    }
}