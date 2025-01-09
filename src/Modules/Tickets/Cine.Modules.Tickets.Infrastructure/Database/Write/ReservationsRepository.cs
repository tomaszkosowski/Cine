using Cine.Modules.Tickets.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write;

internal sealed class ReservationsRepository(WriteContext context) : IReservationsRepository
{
    public async Task AddAsync(Reservation reservation)
    {
        await context.Reservations.AddAsync(reservation);
    }

    public async Task<Reservation?> FindAsync(ReservationId reservationId)
    {
        return await context.Reservations.FindAsync(reservationId);
    }

    public async Task<IReadOnlyList<Reservation>> GetUnpaidReservationsAsync()
    {
        var reservations = await context.Reservations
            .ToListAsync();

        return reservations
            .Where(reservation => reservation.ReservationStatus is Unpaid)
            .ToList();
    }
}