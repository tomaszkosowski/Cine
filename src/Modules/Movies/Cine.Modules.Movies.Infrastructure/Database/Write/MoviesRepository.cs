using Cine.Modules.Movies.Domain;

namespace Cine.Modules.Movies.Infrastructure.Database.Write
{
    internal sealed class MoviesRepository(WriteContext context) : IMoviesRepository
    {
        public async Task AddAsync(Movie movie)
        {
            await context.AddAsync(movie);
        }
    }
}