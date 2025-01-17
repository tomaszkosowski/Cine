using Cine.Modules.Sales.Domain.DiscountSpecifications;

namespace Cine.Modules.Sales.Domain.DiscountStrategies;

internal sealed class PercentageDiscountStrategy(double percentage) : DiscountStrategy
{
    public override double Calculate(ReservationContext reservationContext) => reservationContext.Amount * percentage * 0.01;
}