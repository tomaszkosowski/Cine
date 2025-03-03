using Cine.Modules.Sales.Domain.Events;
using Cine.Shared.Application.Events;

namespace Cine.Modules.Sales.Application.Payments.PaymentConfirmed;

public record PaymentConfirmedNotification(Guid id, PaymentConfirmedDomainEvent domainEvent)
    : DomainEventNotification<PaymentConfirmedDomainEvent>(id, domainEvent);