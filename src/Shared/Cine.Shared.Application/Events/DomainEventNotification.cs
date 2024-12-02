using Cine.Shared.Domain.Events;

namespace Cine.Shared.Application.Events
{
    public record DomainEventNotification<TDomainEvent>(Guid id, TDomainEvent domainEvent)
        : IDomainEventNotification<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        public Guid Id => id;

        public TDomainEvent DomainEvent => domainEvent;
    }
}
