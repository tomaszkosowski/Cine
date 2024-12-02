namespace Cine.Shared.Infrastructure.Events;

public interface IDomainEventsMapper
{
    string GetDomainEventName(Type type);

    Type GetDomainEventType(string name);
}