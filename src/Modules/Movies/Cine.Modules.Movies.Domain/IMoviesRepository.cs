namespace Cine.Modules.Movies.Domain;

public interface IMoviesRepository
{
    Task AddAsync(Movie movie);
}