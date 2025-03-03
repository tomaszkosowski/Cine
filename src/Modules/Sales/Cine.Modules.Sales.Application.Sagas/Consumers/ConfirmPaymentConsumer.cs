using Cine.Modules.Sales.Domain;
using MassTransit;

namespace Cine.Modules.Sales.Application.Sagas.Consumers;

internal record ConfirmPayment(Guid ReservationId);

internal sealed class ConfirmPaymentConsumer(IPaymentRepository paymentRepository) : IConsumer<ConfirmPayment>
{
    public async Task Consume(ConsumeContext<ConfirmPayment> context)
    {
        var reservationId = context.Message.ReservationId;

        var payment = await paymentRepository.GetAsync(ReservationId.Create(reservationId));
        if (payment is null)
        {
            throw new ApplicationException($"Payment with given ReservationId {reservationId} was not found");
        }
        
        payment.ChangeStatus(PaymentStatusType.Confirmed);
    }
}