using Cine.Shared.Domain;

namespace Cine.Shared.Infrastructure.Events;

public abstract record IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = Utc.Now;
}