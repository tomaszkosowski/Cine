using Cine.Shared.Domain.Events;

namespace Cine.Modules.Tickets.Domain.Events;

public record ReservationExpiredDomainEvent(ReservationId ReservationId) : DomainEvent;