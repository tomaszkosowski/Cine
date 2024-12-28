using Cine.Shared.Domain;

namespace Cine.Modules.Shows.Domain;

public record MovieId : TypedId<MovieId>;

public sealed class Movie : Entity, IAggregateRoot
{
    public MovieId MovieId { get; }

    private Movie()
    {
        // Blank for ORM.
    }

    private Movie(MovieId movieId)
    {
        MovieId = movieId;
    }

    public static Movie Create(MovieId movieId) => new(movieId);
}