using Cine.Shared.Domain;

namespace Cine.Modules.Sales.Domain;

public record PaymentId : TypedId<PaymentId>;

public class Payment
{
    #region Properties

    public PaymentId PaymentId { get; }

    public ReservationId ReservationId { get; }

    #endregion

    #region Constructors

    private Payment()
    {
        // Blank for ORM. 
    }

    private Payment(ReservationId reservationId)
    {
        PaymentId = PaymentId.Create();

        ReservationId = reservationId;
    }

    #endregion

    #region Public methods

    public static Payment Create(ReservationId reservationId) => new(reservationId);

    #endregion
}