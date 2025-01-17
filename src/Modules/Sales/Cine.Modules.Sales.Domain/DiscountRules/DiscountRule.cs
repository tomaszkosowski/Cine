using Cine.Modules.Sales.Domain.DiscountSpecifications;

namespace Cine.Modules.Sales.Domain.DiscountRules;

internal abstract class DiscountRule
{
    public abstract double ApplyDiscounts(ReservationContext reservationContext);
}