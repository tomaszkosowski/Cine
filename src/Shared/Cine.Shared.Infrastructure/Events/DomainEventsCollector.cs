using Cine.Shared.Domain;
using Cine.Shared.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Cine.Shared.Infrastructure.Events;

public sealed class DomainEventsCollector<TContext>(TContext context)
    : IDomainEventsCollector where TContext : DbContext
{
    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        var entities = context.ChangeTracker
            .Entries<Entity>().Where(entry
                => entry is not null && entry.Entity.DomainEvents?.Count > 0)
            .ToList();

        var events = entities.SelectMany(entity => entity.Entity.DomainEvents).ToList();

        return events;
    }

    public void ClearAllDomainEvents()
    {
        var entities = context.ChangeTracker
            .Entries<Entity>().Where(entry
                => entry is not null && entry.Entity.DomainEvents?.Count > 0)
            .ToList();

        entities.ForEach(entity => entity.Entity.ClearDomainEvents());
    }
}