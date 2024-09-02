namespace Cine.Shared.Domain.Events
{
    public abstract record DomainEvent : IDomainEvent
    {
        #region Properties

        public Guid EventId { get; set; }

        public DateTime PublishedAt { get; set; }

        #endregion
    }
}
