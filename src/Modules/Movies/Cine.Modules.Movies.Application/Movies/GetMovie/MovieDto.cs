namespace Cine.Modules.Movies.Application.Movies.GetMovie;

public record MovieDto
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required string Genre { get; init; }

    public required TimeSpan Duration { get; init; }

    public required DateTime ReleaseDate { get; init; }

    public required string Directors { get; init; }

    public required string Cast { get; init; }
}