namespace Cine.Shared.Application.Outbox;

public record OutboxMessage
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Type { get; init; }

    public string Content { get; init; }

    public DateTime? ProcessedAt { get; private set; }

    public void SetAsProcessed() => ProcessedAt = DateTime.UtcNow;
};