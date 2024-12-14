using Cine.Modules.Theater.Domain.Events;
using Cine.Shared.Application.Events;

namespace Cine.Modules.Theater.Application.Halls.HallCreated;

public record HallCreatedNotification(Guid id, HallCreatedDomainEvent domainEvent)
    : DomainEventNotification<HallCreatedDomainEvent>(id, domainEvent);