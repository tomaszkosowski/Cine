using Cine.Shared.Application.Outbox;
using MediatR;
using Newtonsoft.Json;

namespace Cine.Shared.Infrastructure.Events
{
    public sealed class DomainEventsDispatcher(IPublisher _publisher, IOutbox _outbox, IDomainEventsCollector _domainEventsCollector) : IDomainEventsDispatcher
    {
        public async Task DispatchEventsAsync()
        {
            List<OutboxMessage> outboxMessages = [];

            var domainEvents = _domainEventsCollector.GetAllDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                _domainEventsCollector.ClearAllDomainEvents();

                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Type = domainEvent.GetType().Name,
                    Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                };

                outboxMessages.Add(outboxMessage);
            }

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }

            foreach (var outboxMessage in outboxMessages)
            {
                _outbox.Add(outboxMessage);
            }
        }
    }
}
