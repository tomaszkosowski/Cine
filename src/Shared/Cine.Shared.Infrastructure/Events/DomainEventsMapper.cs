namespace Cine.Shared.Infrastructure.Events;

public sealed class DomainEventsMapper(Dictionary<string, Type> mappings) : IDomainEventsMapper
{
    public string GetDomainEventName(Type type)
    {
        return mappings.ContainsValue(type) ? mappings.First(kvp => kvp.Value == type).Key : null!;
    }

    public Type GetDomainEventType(string name)
    {
        return mappings.ContainsKey(name) ? mappings.First(kvp => kvp.Key == name).Value : null!;
    }
}