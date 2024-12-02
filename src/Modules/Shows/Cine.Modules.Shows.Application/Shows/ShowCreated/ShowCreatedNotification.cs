using Cine.Modules.Shows.Domain.Events;
using Cine.Shared.Application.Events;

namespace Cine.Modules.Shows.Application.Shows.ShowCreated;

public record ShowCreatedNotification(Guid id, ShowCreatedDomainEvent domainEvent)
    : DomainEventNotification<ShowCreatedDomainEvent>(id, domainEvent)
{
}