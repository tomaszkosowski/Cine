using Cine.Modules.Sales.Domain;
using MassTransit;

namespace Cine.Modules.Sales.Application.Sagas.Consumers;

internal record CancelPayment(Guid ReservationId);

internal sealed class CancelPaymentConsumer(IPaymentRepository paymentRepository) : IConsumer<CancelPayment>
{
    public async Task Consume(ConsumeContext<CancelPayment> context)
    {
        var reservationId = context.Message.ReservationId;

        var payment = await paymentRepository.GetAsync(ReservationId.Create(reservationId));
        if (payment is null)
        {
            throw new ApplicationException($"Payment with given ReservationId {reservationId} was not found");
        }

        payment.ChangeStatus(PaymentStatusType.Canceled);
    }
}