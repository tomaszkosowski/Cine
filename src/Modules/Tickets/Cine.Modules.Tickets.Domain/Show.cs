using Cine.Modules.Tickets.Domain.Events;
using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record ShowId : TypedId<ShowId>;

public record HallId : TypedId<HallId>;

public sealed class Show : Entity, IAggregateRoot
{
    #region Properties

    public ShowId ShowId { get; }

    public HallId HallId { get; }

    public DateTime StartAt { get; }

    #endregion

    #region Constructors

    private Show()
    {
        // Blank for ORM.
    }

    private Show(ShowId showId, HallId hallId, DateTime startAt)
    {
        ShowId = showId;
        HallId = hallId;
        StartAt = startAt;

        AddDomainEvent(new ShowCreatedDomainEvent(ShowId, HallId, StartAt));
    }

    #endregion

    #region Public methods

    public static Show Create(ShowId showId, HallId hallId, DateTime startAt)
        => new(showId, hallId, startAt);

    #endregion
}