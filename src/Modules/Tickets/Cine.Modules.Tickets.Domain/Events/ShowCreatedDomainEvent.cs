using Cine.Shared.Domain.Events;

namespace Cine.Modules.Tickets.Domain.Events;

public record ShowCreatedDomainEvent(Guid ShowId, Guid HallId, DateTime StartAt) : DomainEvent;