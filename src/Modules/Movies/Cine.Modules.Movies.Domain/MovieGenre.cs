using Cine.Shared.Domain;

namespace Cine.Modules.Movies.Domain
{
    //    Action,
    //    Adventure,
    //    Animation,
    //    Comedy,
    //    Crime,
    //    Documentary,
    //    Drama,
    //    Fantasy,
    //    Horror,
    //    Mystery,
    //    Romance,
    //    SciFi,
    //    Thriller

    public record MovieGenre : ValueObject
    {
        public string Genre { get; }

        private MovieGenre(string genre) => Genre = genre;

        public static MovieGenre Of(string genre) => new(genre);

        public static implicit operator MovieGenre(string genre) => Of(genre);

        public override string ToString() => Genre;
    }
}
