using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record MovieId : TypedId<MovieId>;

public sealed class Movie : Entity, IAggregateRoot
{
    public MovieId MovieId { get; }

    public string Title { get; }

    private Movie()
    {
        // Blank for ORM.
    }

    private Movie(MovieId movieId, string title)
    {
        MovieId = movieId;
        Title = title;
    }

    public static Movie Create(MovieId movieId, string title) 
        => new(movieId, title);
}