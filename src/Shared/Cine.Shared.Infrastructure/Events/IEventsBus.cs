namespace Cine.Shared.Infrastructure.Events;

public interface IEventsBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent;

    Task SubscribeAsync<TEvent>(IIntegrationEventHandler<TEvent> handler, CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent;
}