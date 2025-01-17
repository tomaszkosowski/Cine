using Cine.Modules.Sales.Domain.DiscountSpecifications;

namespace Cine.Modules.Sales.Domain.DiscountStrategies;

internal sealed class FixedAmountDiscountStrategy(double discountAmount) : DiscountStrategy
{
    public override double Calculate(ReservationContext reservationContext) => discountAmount;
}