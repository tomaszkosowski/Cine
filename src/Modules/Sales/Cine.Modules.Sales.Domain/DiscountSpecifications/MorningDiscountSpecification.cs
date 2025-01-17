using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class MorningDiscountSpecification(DiscountStrategy discountStrategy)
    : DiscountSpecification(discountStrategy)
{
    public override bool IsSatisfiedBy(ReservationContext reservationContext) =>
        reservationContext.StartAt.Hour is >= 8 and < 12;
}