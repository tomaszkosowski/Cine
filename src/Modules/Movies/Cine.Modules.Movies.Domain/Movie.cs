using Cine.Modules.Movies.Domain.Events;
using Cine.Shared.Domain;
using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Movies.Domain;

public record MovieId : TypedId<MovieId>;

public sealed class Movie : Entity, IAggregateRoot
{
    #region Fields

    private readonly List<Person> _directors = [];
    private readonly List<Person> _cast = [];

    #endregion

    #region Properties

    public MovieId MovieId { get; }

    public string Title { get; }

    public string Description { get; }

    public MovieGenre MovieGenre { get; }

    public TimeOnly Duration { get; }

    public DateOnly ReleaseDate { get; }

    public IReadOnlyCollection<Person> Directors => _directors.AsReadOnly();

    public IReadOnlyCollection<Person> Cast => _cast.AsReadOnly();

    #endregion

    #region Constructors

    private Movie()
    {
        // Blank for ORM.
    }

    private Movie(string title, string description, MovieGenre movieGenre, TimeOnly duration, DateOnly releaseDate, IReadOnlyList<Person> directors, IReadOnlyList<Person> cast)
    {
        CheckRule(new EnsureNotEmptyRule(title, nameof(title)));

        MovieId = MovieId.Create();

        Title = title;
        Description = description;
        MovieGenre = movieGenre;
        Duration = duration;
        ReleaseDate = releaseDate;

        _directors.AddRange(directors);
        _cast.AddRange(cast);

        AddDomainEvent(new MovieCreatedDomainEvent(MovieId));
    }

    #endregion

    #region Public methods

    public void AddDirectors(List<Person> people)
    {
        if (people is { Count: > 0 })
        {
            _directors.AddRange(people);

            AddDomainEvent(new MovieUpdatedDomainEvent(MovieId));
        }
    }

    public void AddCast(List<Person> people)
    {
        if (people is { Count: > 0 })
        {
            _cast.AddRange(people);

            AddDomainEvent(new MovieUpdatedDomainEvent(MovieId));
        }
    }

    public static Movie Create(string title, string description, MovieGenre movieGenre, TimeOnly duration, DateOnly releaseDate, IReadOnlyList<Person> directors, IReadOnlyList<Person> cast)
        => new(title, description, movieGenre, duration, releaseDate, directors, cast);

    #endregion
}