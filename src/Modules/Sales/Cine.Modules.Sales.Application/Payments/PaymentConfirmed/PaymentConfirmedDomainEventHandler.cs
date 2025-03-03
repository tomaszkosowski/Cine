using Cine.Modules.Sales.Domain.Events;
using MediatR;

namespace Cine.Modules.Sales.Application.Payments.PaymentConfirmed;

internal sealed class PaymentConfirmedDomainEventHandler(IPublisher publisher) : INotificationHandler<PaymentConfirmedDomainEvent>
{
    public async Task Handle(PaymentConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publisher.Publish(new PaymentConfirmedNotification(notification.EventId, notification),
            cancellationToken);
    }
}