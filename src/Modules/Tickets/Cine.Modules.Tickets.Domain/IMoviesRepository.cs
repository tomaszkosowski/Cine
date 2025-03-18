namespace Cine.Modules.Tickets.Domain;

public interface IMoviesRepository
{
    Task AddAsync(Movie movie);
}