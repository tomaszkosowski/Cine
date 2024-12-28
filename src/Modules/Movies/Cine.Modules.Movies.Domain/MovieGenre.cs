using Cine.Shared.Domain;

namespace Cine.Modules.Movies.Domain;

public record MovieGenre : ValueObject
{
    #region Properties

    public string Genre { get; }

    #endregion

    #region Constructor

    private MovieGenre(string genre) => Genre = genre;

    #endregion

    #region Public methods

    public static MovieGenre Of(string genre) => new(genre);

    public static implicit operator MovieGenre(string genre) => Of(genre);

    public override string ToString() => Genre;

    #endregion
}