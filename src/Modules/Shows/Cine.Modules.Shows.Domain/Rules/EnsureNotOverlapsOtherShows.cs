using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Shows.Domain.Rules;

internal sealed class EnsureNotOverlapsOtherShows(Schedule scheduledAt, IReadOnlyList<Show> otherShows) : IBusinessRule
{
    public string Message => "Schedule overlaps with other show.";

    public bool IsBroken()
    {
        foreach (var otherShow in otherShows)
        {
            if (scheduledAt.IsOverlapping(otherShow.ScheduledAt))
            {
                return true;
            }
        }

        return false;
    }
}