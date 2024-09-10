using Cine.Shared.Domain.Events;

namespace Cine.Shared.Application.Events
{
    public class DomainEventNotification<TDomainEvent>(Guid _id, TDomainEvent _domainEvent)
        : IDomainEventNotification<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        public Guid Id => _id;

        public TDomainEvent DomainEvent => _domainEvent;
    }
}
