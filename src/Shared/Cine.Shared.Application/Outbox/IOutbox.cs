namespace Cine.Shared.Application.Outbox;

public interface IOutbox
{
    void Add(OutboxMessage message);

    void AddRange(IEnumerable<OutboxMessage> messages);

    Task SaveAsync();
}