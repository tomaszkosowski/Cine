using Cine.Modules.Tickets.Domain.Events;
using Cine.Shared.Application.Events;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationConfirmed;

public record ReservationConfirmedNotification(Guid id, ReservationConfirmedDomainEvent domainEvent)
    : DomainEventNotification<ReservationConfirmedDomainEvent>(id, domainEvent);