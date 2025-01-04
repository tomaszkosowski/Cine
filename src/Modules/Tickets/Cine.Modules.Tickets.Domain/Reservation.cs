using System.Collections.Immutable;
using Cine.Modules.Tickets.Domain.Events;
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

    public IReadOnlyList<Seat> Seats => _seats.ToImmutableList();

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

    private Reservation(DateTime reservedAt)
    {
        // CheckRule(new EnsureNotPastRule(reservedAt, nameof(reservedAt)));

        ReservationId = ReservationId.Create();

        ReservationStatus = new Unpaid(reservedAt);

        AddDomainEvent(new ReservationCreatedDomainEvent());
    }

    #endregion

    #region Public methods

    public static Reservation Create() => new(Utc.Now);

    public void AddSeat(Seat seat)
    {
        _seats.Add(seat);
        
        AddDomainEvent(new SeatReservedDomainEvent());
    }

    public void Expire()
    {
        ReservationStatus = ReservationStatus.AdvanceTo<Expired>();

        AddDomainEvent(new ReservationExpiredDomainEvent(ReservationId));
    }

    public void Pay()
    {
        ReservationStatus = ReservationStatus.AdvanceTo<Paid>();

        AddDomainEvent(new ReservationPaidDomainEvent(ReservationId));
    }

    #endregion
}