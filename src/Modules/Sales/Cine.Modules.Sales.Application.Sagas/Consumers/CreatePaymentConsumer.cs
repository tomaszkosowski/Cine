using Cine.Modules.Sales.Application.Sagas.ApiClients;
using Cine.Modules.Sales.Domain;
using Cine.Modules.Sales.Domain.DiscountRules;
using Cine.Modules.Sales.Domain.DiscountSpecifications;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Cine.Modules.Sales.Application.Sagas.Consumers;

public record CreatePayment(Guid ReservationId);

internal sealed class CreatePaymentConsumer(
    IConfiguration configuration,
    ITicketsApiClient ticketsApiClient,
    IPaymentRepository paymentRepository) : IConsumer<CreatePayment>
{
    public async Task Consume(ConsumeContext<CreatePayment> context)
    {
        var seatPrice = double.Parse(configuration["Features:Payments:SeatPrice"]!);
        var reservationDto = await ticketsApiClient.GetReservationAsync(context.Message.ReservationId);

        var amount = reservationDto.SeatsCount * seatPrice;
        var payment = Payment.Create(ReservationId.Create(context.Message.ReservationId), amount);

        var discountRule = new MondaySpecialDiscountRule();
        var discount = discountRule.ApplyDiscounts(ReservationContext.Create(amount, reservationDto.ReservedAt!.Value, reservationDto.SeatsCount));
        payment.ApplyDiscount(discount);

        await paymentRepository.AddAsync(payment);
        
        await context.Publish(new PaymentCreated(context.Message.ReservationId));
    }
}