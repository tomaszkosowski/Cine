namespace Cine.Modules.Tickets.Domain;

public interface IReservationsRepository
{
    Task AddAsync(Reservation reservation);
    Task<IReadOnlyList<Reservation>> GetUnpaidReservationsAsync();
}