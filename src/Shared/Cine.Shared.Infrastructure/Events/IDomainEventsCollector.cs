using Cine.Shared.Domain.Events;

namespace Cine.Shared.Infrastructure.Events
{
    public interface IDomainEventsCollector
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}
