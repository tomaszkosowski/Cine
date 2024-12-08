using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record ShowId : TypedId<ShowId>;

public sealed class Show : Entity, IAggregateRoot
{
    #region Properties

    public ShowId ShowId { get; }

    #endregion

    #region Constructors

    private Show()
    {
        // Blank for ORM..
    }

    private Show(ShowId showId)
    {
        ShowId = showId;
    }

    #endregion

    #region Public methods

    public static Show Create(ShowId showId) => new(showId);

    #endregion
}