namespace Cine.Shared.Infrastructure.Events;

public interface IIntegrationEventHandler
{
}

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IIntegrationEvent
{
    Task HandleAsync(TIntegrationEvent @event);
}