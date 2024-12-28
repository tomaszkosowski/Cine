using Cine.Modules.Shows.Domain.Events;
using Cine.Modules.Shows.Domain.Rules;
using Cine.Shared.Domain;

namespace Cine.Modules.Shows.Domain;

public record ShowId : TypedId<ShowId>;

public sealed class Show : Entity, IAggregateRoot
{
    public ShowId ShowId { get; }

    public HallId HallId { get; }

    public MovieId MovieId { get; }

    public Schedule ScheduledAt { get; }

    private Show()
    {
        // Blank for ORM.
    }

    private Show(HallId hallId, MovieId movieId, Schedule scheduledAt, IReadOnlyList<Show> otherShows)
    {
        CheckRule(new EnsureNotOverlapsOtherShows(scheduledAt, otherShows));

        ShowId = ShowId.Create();

        HallId = hallId;
        MovieId = movieId;
        ScheduledAt = scheduledAt;

        AddDomainEvent(new ShowCreatedDomainEvent(ShowId));
    }

    public static Show Create(HallId hallId, MovieId movieId, Schedule scheduledAt, IReadOnlyList<Show> otherShows)
        => new(hallId, movieId, scheduledAt, otherShows);
}