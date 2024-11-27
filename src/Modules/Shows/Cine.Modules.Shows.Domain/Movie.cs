using Cine.Shared.Domain;

namespace Cine.Modules.Shows.Domain
{
    public record MovieId : TypedId<MovieId>;

    public sealed class Movie
    {
        public MovieId MovieId { get; }

        public string Title { get; }
    }
}
