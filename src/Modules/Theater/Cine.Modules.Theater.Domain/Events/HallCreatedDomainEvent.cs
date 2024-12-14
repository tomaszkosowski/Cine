using Cine.Shared.Domain.Events;

namespace Cine.Modules.Theater.Domain.Events;

public record HallCreatedDomainEvent(HallId HallId) : DomainEvent;