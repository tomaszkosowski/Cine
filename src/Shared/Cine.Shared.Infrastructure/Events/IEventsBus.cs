namespace Cine.Shared.Infrastructure.Events
{
    public interface IEventsBus
    {
        void Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent;

        void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler) where TEvent : IIntegrationEvent;
    }
}
