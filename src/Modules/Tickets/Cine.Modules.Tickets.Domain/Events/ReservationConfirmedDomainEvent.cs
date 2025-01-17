using Cine.Shared.Domain.Events;
using MediatR;

namespace Cine.Modules.Tickets.Domain.Events;

public record ReservationConfirmedDomainEvent(ReservationId ReservationId) : DomainEvent;