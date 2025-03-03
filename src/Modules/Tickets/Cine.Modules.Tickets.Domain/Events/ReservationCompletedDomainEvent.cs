using Cine.Shared.Domain.Events;

namespace Cine.Modules.Tickets.Domain.Events;

public record ReservationCompletedDomainEvent(ReservationId ReservationId) : DomainEvent;