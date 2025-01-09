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

    #endregion

    #region Constructors

    private Show()
    {
        // Blank for ORM.
    }

    private Show(ShowId showId, HallId hallId)
    {
        ShowId = showId;
        HallId = hallId;

        AddDomainEvent(new ShowCreatedDomainEvent(ShowId, HallId));
    }

    #endregion

    #region Public methods

    public static Show Create(ShowId showId, HallId hallId) => new(showId, hallId);

    #endregion
}