using Cine.Shared.Domain;

namespace Cine.Modules.Theater.Domain
{
    public record HallId : TypedId<HallId>;

    public sealed class Hall : Entity, IAggregateRoot
    {
        public HallId HallId { get; }

        public string Name { get; }

        public IReadOnlyCollection<Seat> Seats { get; } = [];
    }
}
