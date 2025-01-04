using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Shows.Domain.Rules;

internal sealed class EnsureNotOverlapsOtherShows(HallId hallId, Schedule scheduledAt, IReadOnlyList<ShowInfo> otherShows)
    : IBusinessRule
{
    public string Message => "Schedule overlaps with other show.";

    public bool IsBroken()
    {
        return otherShows.Where(otherShow => hallId == otherShow.HallId)
            .Any(otherShow => scheduledAt.IsOverlapping(otherShow.ScheduledAt));
    }
}