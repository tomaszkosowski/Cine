namespace Cine.Modules.Shows.Domain;

public interface IShowsRepository
{
    Task AddAsync(Show show);
}