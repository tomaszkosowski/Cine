using Cine.Modules.Tickets.Domain.Events;
using Cine.Modules.Tickets.Domain.Rules;
using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record ReservationId : TypedId<ReservationId>;

public sealed class Reservation : Entity, IAggregateRoot
{
    #region Fields

    private readonly List<Seat> _seats = [];

    #endregion

    #region Properties

    public ReservationId ReservationId { get; private set; }

    public ShowId ShowId { get; private set; }

    public IReadOnlyList<Seat> Seats => _seats;

    public IReservationStatus ReservationStatus { get; private set; }

    private ReservationStatusRepresentation ReservationStatusRepresentation
    {
        get => ReservationStatus.Convert();
        set => ReservationStatus = value.Convert();
    }

    #endregion

    #region Constructors

    private Reservation()
    {
        // Blank for ORM.
    }

    private Reservation(ShowId showId, DateTime reservedAt)
    {
        ReservationId = ReservationId.Create();
        ShowId = showId;

        ReservationStatus = new Unpaid(reservedAt);

        AddDomainEvent(new ReservationCreatedDomainEvent());
    }

    #endregion

    #region Public methods

    public static Reservation Create(ShowId showId) => new(showId, Utc.Now);

    public void AddSeat(Seat seat)
    {
        CheckRule(new EnsureSeatNotSeparatedRule(seat, Seats));

        seat.ChangeStatus(SeatStatusType.Reserved);

        _seats.Add(seat);
    }

    public void RemoveSeat(Seat seat)
    {
        CheckRule(new EnsureSeatNotAdjacentRule(seat, Seats));

        seat.ChangeStatus(SeatStatusType.Open);

        _seats.Remove(seat);
    }

    public void Confirm()
    {
        CheckRule(new EnsureReservationNotEmpty(this));

        ReservationStatus = ReservationStatus.AdvanceTo<Confirmed>();

        AddDomainEvent(new ReservationConfirmedDomainEvent(ReservationId));
    }

    public void Expire()
    {
        ReservationStatus = ReservationStatus.AdvanceTo<Expired>();

        AddDomainEvent(new ReservationExpiredDomainEvent(ReservationId));
    }

    public void Complete()
    {
        ReservationStatus = ReservationStatus.AdvanceTo<Completed>();

        AddDomainEvent(new ReservationCompletedDomainEvent(ReservationId));
    }

    #endregion
}