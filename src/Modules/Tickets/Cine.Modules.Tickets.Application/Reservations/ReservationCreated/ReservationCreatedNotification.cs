using Cine.Modules.Tickets.Domain.Events;
using Cine.Shared.Application.Events;

namespace Cine.Modules.Tickets.Application.Reservations.ReservationCreated;

public record ReservationCreatedNotification(Guid id, ReservationCreatedDomainEvent domainEvent)
    : DomainEventNotification<ReservationCreatedDomainEvent>(id, domainEvent);