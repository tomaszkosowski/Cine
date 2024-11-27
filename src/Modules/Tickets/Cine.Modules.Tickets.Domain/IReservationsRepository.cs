namespace Cine.Modules.Tickets.Domain;

public interface IReservationsRepository
{
    Task<IReadOnlyList<Reservation>> GetUnpaidReservationsAsync();
}