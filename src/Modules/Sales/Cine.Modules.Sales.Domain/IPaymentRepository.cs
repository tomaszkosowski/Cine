namespace Cine.Modules.Sales.Domain;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);

    Task<Payment?> GetAsync(ReservationId reservationId);
}