using Cine.Shared.Domain;

namespace Cine.Modules.Theater.Domain
{
    public record HallId : TypedId<HallId>;

    public sealed class Hall : Entity, IAggregateRoot
    {
        public HallId HallId { get; }

        public string Name { get; }

        public IReadOnlyCollection<Seat> Seats { get; private set; } = [];

        private Hall()
        {
            // Blank for ORM..
        }

        private Hall(string name, List<Seat> seats)
        {
            HallId = HallId.Create();

            Name = name;
            Seats = seats;
        }

        public static Hall Create(string name, List<Seat> seats) => new(name, seats);

        public void AssignSeats(List<Seat> seats) => Seats = seats;
    }
}