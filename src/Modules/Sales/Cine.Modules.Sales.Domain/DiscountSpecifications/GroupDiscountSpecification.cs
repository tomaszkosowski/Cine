using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal class GroupDiscountSpecification(DiscountStrategy discountStrategy)
    : DiscountSpecification(discountStrategy)
{
    public override bool IsSatisfiedBy(ReservationContext reservationContext)
        => reservationContext.SeatsCount > 4;
}