using Cine.Modules.Tickets.Domain;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write;

internal sealed class MoviesRepository(WriteContext context) : IMoviesRepository
{
    public async Task AddAsync(Movie movie) 
        => await context.Movies.AddAsync(movie);
}