namespace Cine.Modules.Tickets.Domain;

public interface IReservationsRepository
{
    Task AddAsync(Reservation reservation);
    Task<Reservation?> FindAsync(ReservationId reservationId);
    Task<IReadOnlyList<Reservation>> GetUnpaidReservationsAsync();
}