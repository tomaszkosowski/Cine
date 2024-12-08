using Cine.Shared.Domain;

namespace Cine.Modules.Theater.Domain
{
    public record SeatId : TypedId<SeatId>;

    public sealed class Seat : Entity, IAggregateRoot
    {
        #region Properties

        public SeatId SeatId { get; }

        public HallId HallId { get; }

        public string Row { get; }

        public int Number { get; }

        public SeatType Type { get; }

        #endregion

        #region Constructors

        private Seat()
        {
            // Blank for ORM.
        }

        private Seat(HallId hallId, string row, int number, SeatType type)
        {
            SeatId = SeatId.Create();

            HallId = hallId;
            Row = row;
            Number = number;
            Type = type;
        }

        #endregion

        #region Public methods

        public static Seat CreateRegular(HallId hallId, string row, int number) =>
            new(hallId, row, number, SeatType.Regular);

        public static Seat CreatePremium(HallId hallId, string row, int number) =>
            new(hallId, row, number, SeatType.Premium);

        #endregion
    }
}