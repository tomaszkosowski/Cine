using Cine.Shared.Domain.Events;

namespace Cine.Modules.Tickets.Domain.Events;

public record ReservationCreatedDomainEvent(IReadOnlyList<Seat> SeatsToBeReserved) : DomainEvent;