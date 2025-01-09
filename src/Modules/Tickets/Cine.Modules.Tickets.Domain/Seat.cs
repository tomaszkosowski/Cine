using Cine.Modules.Tickets.Domain.Events;
using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record SeatId : TypedId<SeatId>;

public sealed class Seat : Entity, IAggregateRoot
{
    #region Properties

    public SeatId SeatId { get; }

    public ShowId ShowId { get; }

    public string Row { get; }

    public int Number { get; }

    public SeatStatusType Status { get; private set; }

    public ReservationId ReservationId { get; }

    public Reservation? Reservation { get; }

    #endregion

    #region Constructor

    private Seat()
    {
        // Blank for ORM.
    }

    private Seat(SeatId seatId, ShowId showId, string row, int number, SeatStatusType status)
    {
        SeatId = seatId;
        ShowId = showId;
        Row = row;
        Number = number;
        Status = status;
    }

    #endregion

    #region Public methods

    public static Seat Create(SeatId seatId, ShowId showId, string row, int number) =>
        new(seatId, showId, row, number, SeatStatusType.Available);

    public void ChangeStatus(SeatStatusType status)
    {
        switch (status)
        {
            case var _ when status == SeatStatusType.Available:
            {
                Status = SeatStatusType.Available;
                AddDomainEvent(new SeatReleasedDomainEvent());
                break;
            }

            case var _ when status == SeatStatusType.Reserved:
            {
                Status = SeatStatusType.Reserved;
                AddDomainEvent(new SeatReservedDomainEvent());
                break;
            }

            case var _ when status == SeatStatusType.Sold:
            {
                Status = SeatStatusType.Sold;
                AddDomainEvent(new SeatSoldDomainEvent());
                break;
            }
        }
    }

    #endregion
}