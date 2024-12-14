namespace Cine.Modules.Shows.Domain;

public interface IMoviesRepository
{
    Task AddAsync(Movie movie);
}