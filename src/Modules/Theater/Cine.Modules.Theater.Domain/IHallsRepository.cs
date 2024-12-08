namespace Cine.Modules.Theater.Domain;

public interface IHallsRepository
{
    Task AddAsync(Hall hall);
}