using Cine.Modules.Sales.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cine.Modules.Sales.Infrastructure.Database.Write;

internal sealed class PaymentRepository(WriteContext context) : IPaymentRepository
{
    public async Task AddAsync(Payment payment)
        => await context.Payments.AddAsync(payment);

    public async Task<Payment?> GetAsync(ReservationId reservationId) =>
        await context.Payments.FirstOrDefaultAsync(payment => payment.ReservationId == reservationId);
}