using Cine.Shared.Application.Outbox;
using MediatR;
using Newtonsoft.Json;

namespace Cine.Shared.Infrastructure.Events
{
    public sealed class DomainEventsDispatcher(IPublisher publisher, IOutbox outbox, IDomainEventsCollector domainEventsCollector) : IDomainEventsDispatcher
    {
        public async Task DispatchEventsAsync()
        {
            List<OutboxMessage> outboxMessages = [];

            var domainEvents = domainEventsCollector.GetAllDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                domainEventsCollector.ClearAllDomainEvents();

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
                await publisher.Publish(domainEvent);
            }

            foreach (var outboxMessage in outboxMessages)
            {
                outbox.Add(outboxMessage);
            }
        }
    }
}
