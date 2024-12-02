namespace Cine.Modules.Tickets.Domain;

public interface IShowsRepository
{
    Task AddAsync(Show show);

    Task<bool> ExistsAsync(ShowId showId);
}