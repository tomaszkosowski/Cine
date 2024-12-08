namespace Cine.Modules.Theater.Domain;

public interface ISeatsRepository
{
    Task AddAsync(Seat seat);
}