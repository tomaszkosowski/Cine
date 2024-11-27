using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Shows.Domain.Rules
{
    internal sealed class EnsureNotOverlapsOtherShows(Schedule ScheduledAt, IReadOnlyList<Show> OtherShows) : IBusinessRule
    {
        public string Message => "Schedule overlaps with other show.";

        public bool IsBroken()
        {
            foreach (var otherShow in OtherShows)
            {
                if (ScheduledAt.IsOverlapping(otherShow.ScheduledAt))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
