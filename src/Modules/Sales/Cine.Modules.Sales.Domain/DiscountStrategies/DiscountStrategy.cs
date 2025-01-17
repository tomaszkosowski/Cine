using Cine.Modules.Sales.Domain.DiscountSpecifications;

namespace Cine.Modules.Sales.Domain.DiscountStrategies;

internal abstract class DiscountStrategy
{
    public abstract double Calculate(ReservationContext reservationContext);
};