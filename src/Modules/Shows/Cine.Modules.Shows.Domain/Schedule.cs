using Cine.Shared.Domain;
using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Shows.Domain
{
    public record Schedule : ValueObject
    {
        public DateTime StartAt { get; }

        public TimeSpan Duration { get; }

        public DateTime EndAt { get; }

        private Schedule(DateTime startAt, TimeSpan duration)
        {
            CheckRule(new EnsureNotPastRule(startAt, nameof(startAt)));
            CheckRule(new EnsureNotZeroRule(duration, nameof(duration)));
            CheckRule(new EnsureNotNegativeRule(duration, nameof(duration)));

            StartAt = startAt;
            Duration = duration;

            EndAt = StartAt.Add(duration);
        }

        public bool IsOverlapping(Schedule other)
        {
            return StartAt < other.EndAt && other.StartAt < EndAt;
        }

        public static Schedule Create(DateTime startAt, TimeSpan duration)
            => new(startAt, duration);
    }
}
