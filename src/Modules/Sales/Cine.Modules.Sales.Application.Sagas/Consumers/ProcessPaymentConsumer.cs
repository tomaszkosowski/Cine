using Cine.Modules.Sales.Domain;
using MassTransit;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Sales.Application.Sagas.Consumers;

internal record ProcessPayment(Guid ReservationId);

internal sealed class ProcessPaymentConsumer(IPaymentRepository paymentRepository) : IConsumer<ProcessPayment>
{
    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        var reservationId = context.Message.ReservationId;

        var payment = await paymentRepository.GetAsync(ReservationId.Create(reservationId));
        if (payment is null)
        {
            throw new ApplicationException($"Payment with given ReservationId {reservationId} was not found");
        }

        var oneOf = await ProcessPayment();
        
        await oneOf.Match(
            async success => await context.Publish(new PaymentSucceeded(reservationId)),
            async failure => await context.Publish(new PaymentFailed(reservationId)));
    }

    private static async Task<OneOf<Success, Error>> ProcessPayment()
    {
        const bool IsSucceeded = true;

        await Task.Delay(5000);
        return IsSucceeded ? new Success() : new Error();
    }
}