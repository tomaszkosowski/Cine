namespace Cine.Modules.Tickets.Domain;

public interface IHallsRepository
{
    Task AddAsync(Hall hall);
}