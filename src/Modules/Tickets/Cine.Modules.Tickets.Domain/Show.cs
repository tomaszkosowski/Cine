using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record ShowId : TypedId<ShowId>;

public sealed class Show : Entity, IAggregateRoot
{
    #region Properties

    public ShowId ShowId { get; }

    #endregion
}