using System.Reflection;
using Cine.Shared.Domain.Events;

namespace Cine.Shared.Infrastructure.Events;

public static class AssemblyExtensions
{
    public static Dictionary<string, Type> DiscoverDomainEventsMappings<TDomainAssemblyInterface>()
    {
        return Assembly.GetAssembly(typeof(TDomainAssemblyInterface))?.GetTypes()
            .Where(type => typeof(DomainEvent).IsAssignableFrom(type))
            .ToDictionary(type => type.Name, type => type) ?? [];
    }
}