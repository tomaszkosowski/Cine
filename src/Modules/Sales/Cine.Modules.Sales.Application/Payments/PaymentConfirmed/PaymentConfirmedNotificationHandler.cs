using Cine.Modules.Sales.IntegrationEvents.Payments;
using Cine.Shared.Infrastructure.Events;
using MediatR;

namespace Cine.Modules.Sales.Application.Payments.PaymentConfirmed;

public class PaymentConfirmedNotificationHandler(IEventsBus eventsBus)
    : INotificationHandler<PaymentConfirmedNotification>
{
    public async Task Handle(PaymentConfirmedNotification notification, CancellationToken cancellationToken)
    {
        await eventsBus.PublishAsync(new PaymentConfirmedIntegrationEvent(notification.DomainEvent.ReservationId),
            cancellationToken);
    }
}