using Cine.Modules.Sales.Domain.DiscountSpecifications;

namespace Cine.Modules.Sales.Domain.DiscountStrategies;

internal sealed class EmptyDiscountStrategy : DiscountStrategy
{
    public override double Calculate(ReservationContext reservationContext) => 0.0;
}