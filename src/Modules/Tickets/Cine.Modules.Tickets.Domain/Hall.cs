using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record HallId : TypedId<HallId>;

public sealed class Hall
{
    #region Properties

    public HallId HallId { get; }

    public string Name { get; }

    #endregion

    #region Constructors

    private Hall()
    {
        // Blank for ORM.
    }

    private Hall(HallId hallId, string name)
    {
        HallId = hallId;
        Name = name;
    }

    #endregion

    #region Public methods

    public static Hall Create(HallId hallId, string name)
        => new(hallId, name);

    #endregion
}