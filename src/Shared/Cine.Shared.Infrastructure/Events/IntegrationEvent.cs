namespace Cine.Shared.Infrastructure.Events
{
    public abstract record IntegrationEvent(Guid Id, DateTime CreatedAt) : IIntegrationEvent;
}
