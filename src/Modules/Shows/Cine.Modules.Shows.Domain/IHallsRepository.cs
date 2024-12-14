namespace Cine.Modules.Shows.Domain;

public interface IHallsRepository
{
    Task AddAsync(Hall hall);
}