using Cine.Shared.Domain.Events;

namespace Cine.Modules.Sales.Domain.Events;

public record PaymentConfirmedDomainEvent(ReservationId ReservationId) : DomainEvent;