using Cine.Modules.Sales.Domain.DiscountSpecifications;

namespace Cine.Modules.Sales.Domain.DiscountRules;

public abstract class DiscountRule
{
    public abstract double ApplyDiscounts(ReservationContext reservationContext);
}