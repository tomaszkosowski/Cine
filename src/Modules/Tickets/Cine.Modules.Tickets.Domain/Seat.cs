using Cine.Modules.Tickets.Domain.Events;
using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record SeatId : TypedId<SeatId>;

public sealed class Seat : Entity, IAggregateRoot
{
    #region Properties

    public SeatId SeatId { get; }

    public SeatStatusType Status { get; private set; }

    public ReservationId ReservationId { get; }
    
    public Reservation? Reservation { get; }

    #endregion

    #region Constructor

    private Seat()
    {
        // Blank for ORM
    }

    private Seat(SeatStatusType status)
    {
        SeatId = SeatId.Create();

        Status = status;
    }

    #endregion

    #region Public methods

    public static Seat Create() => new(SeatStatusType.Available);

    public void ChangeStatus(SeatStatusType status)
    {
        switch (status)
        {
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