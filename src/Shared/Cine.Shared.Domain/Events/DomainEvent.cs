namespace Cine.Shared.Domain.Events
{
    public abstract record DomainEvent : IDomainEvent
    {
        #region Properties

        public Guid EventId { get; } = Guid.NewGuid();

        public DateTime PublishedAt { get; } = Utc.Now;

        #endregion
    }
}
