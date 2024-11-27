using Cine.Modules.Tickets.Domain.Events;
using Cine.Shared.Domain;
using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Tickets.Domain;

public record ReservationId : TypedId<ReservationId>;

public sealed class Reservation : Entity, IAggregateRoot
{
    #region Properties

    public ReservationId ReservationId { get; private set; }

    public IReadOnlyList<Seat> Seats { get; } = [];

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
        // Only for ORM.
    }

    private Reservation(List<Seat> seats, DateTime reservedAt)
    {
        CheckRule(new EnsureNotEmptyCollectionRule<Seat>(seats, nameof(seats)));
        CheckRule(new EnsureNotPastRule(reservedAt, nameof(reservedAt)));

        ReservationId = ReservationId.Create();

        ReservationStatus = new Unpaid(reservedAt);

        AddDomainEvent(new ReservationCreatedDomainEvent(seats));
    }

    #endregion

    #region Public methods

    public static Reservation Create(List<Seat> seats) => new(seats, Utc.Now);

    public void Expire()
    {
        ReservationStatus = new Expired(Utc.Now);

        AddDomainEvent(new ReservationExpiredDomainEvent(ReservationId));
    }

    public void Pay()
    {
        ReservationStatus = new Paid(DateTime.UtcNow);

        AddDomainEvent(new ReservationPaidDomainEvent(ReservationId));
    }

    #endregion
}