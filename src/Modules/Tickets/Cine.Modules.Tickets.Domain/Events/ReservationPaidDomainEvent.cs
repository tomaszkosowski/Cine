using Cine.Shared.Domain.Events;

namespace Cine.Modules.Tickets.Domain.Events;

public record ReservationPaidDomainEvent(ReservationId ReservationId) : DomainEvent;