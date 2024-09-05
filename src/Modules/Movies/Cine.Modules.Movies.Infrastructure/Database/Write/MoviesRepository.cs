using Cine.Modules.Movies.Domain;

namespace Cine.Modules.Movies.Infrastructure.Database.Write
{
    internal sealed class MoviesRepository(WriteContext _context) : IMoviesRepository
    {
        public async Task AddAsync(Movie movie)
        {
            await _context.AddAsync(movie);
        }
    }
}
