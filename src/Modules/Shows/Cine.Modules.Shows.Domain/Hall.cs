using Cine.Shared.Domain;

namespace Cine.Modules.Shows.Domain;

public record HallId : TypedId<HallId>;

public sealed class Hall : Entity, IAggregateRoot
{
    public HallId HallId { get; }

    private Hall()
    {
        // Blank for ORM.
    }

    private Hall(HallId hallId)
    {
        HallId = hallId;
    }

    public static Hall Create(HallId hallId) => new(hallId);
}