using Cine.Modules.Tickets.Infrastructure.Database.Write;
using Cine.Shared.Application.Outbox;

namespace Cine.Modules.Tickets.Infrastructure.Outbox
{
    internal sealed class OutboxAccessor(WriteContext _context) : IOutbox
    {
        public void Add(OutboxMessage message)
        {
            _context.OutboxMessages.Add(message);
        }

        public void AddRange(IEnumerable<OutboxMessage> messages)
        {
            _context.OutboxMessages.AddRange(messages);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
