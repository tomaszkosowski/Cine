using Cine.Modules.Sales.Domain.DiscountRules;
using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.Events;
using Cine.Shared.Domain;

namespace Cine.Modules.Sales.Domain;

public record PaymentId : TypedId<PaymentId>;

public class Payment : Entity, IAggregateRoot
{
    #region Properties

    public PaymentId PaymentId { get; }

    public ReservationId ReservationId { get; }

    public double Amount { get; private set; }

    public PaymentStatusType Status { get; private set; }

    #endregion

    #region Constructors

    private Payment()
    {
        // Blank for ORM. 
    }

    private Payment(ReservationId reservationId, double amount)
    {
        PaymentId = PaymentId.Create();

        ReservationId = reservationId;
        Amount = amount;
        Status = PaymentStatusType.Pending;

        AddDomainEvent(new PaymentCreatedDomainEvent());
    }

    #endregion

    #region Public methods

    public static Payment Create(ReservationId reservationId, double amount) => new(reservationId, amount);

    public void ApplyDiscount(double amount)
    {
        Amount = amount;

        AddDomainEvent(new PaymentDiscountAppliedDomainEvent());
    }

    public void ChangeStatus(PaymentStatusType status)
    {
        switch (status)
        {
            case var _ when status == PaymentStatusType.Confirmed:
            {
                Status = PaymentStatusType.Confirmed;
                AddDomainEvent(new PaymentConfirmedDomainEvent(ReservationId));
                break;
            }

            case var _ when status == PaymentStatusType.Canceled:
            {
                Status = PaymentStatusType.Canceled;
                AddDomainEvent(new PaymentCanceledDomainEvent());
                break;
            }
        }
    }

    #endregion
}