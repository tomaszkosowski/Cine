using Cine.Modules.Shows.Infrastructure.Database.Write;
using Cine.Shared.Application.Outbox;

namespace Cine.Modules.Shows.Infrastructure.Outbox
{
    internal sealed class OutboxAccessor(WriteContext context) : IOutbox
    {
        public void Add(OutboxMessage message)
        {
            context.OutboxMessages.Add(message);
        }

        public void AddRange(IEnumerable<OutboxMessage> messages)
        {
            context.OutboxMessages.AddRange(messages);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
