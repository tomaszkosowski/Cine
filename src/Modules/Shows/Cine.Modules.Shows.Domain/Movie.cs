using Cine.Shared.Domain;

namespace Cine.Modules.Shows.Domain;

public record MovieId : TypedId<MovieId>;

public sealed class Movie : Entity, IAggregateRoot
{
    public MovieId MovieId { get; }

    public TimeOnly Duration { get; }

    private Movie()
    {
        // Blank for ORM.
    }

    private Movie(MovieId movieId, TimeOnly duration)
    {
        MovieId = movieId;
        Duration = duration;
    }

    public static Movie Create(MovieId movieId, TimeOnly duration) 
        => new(movieId, duration);
}