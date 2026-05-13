using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.DiscountStrategies;
using Cine.Shared.Domain.Specifications;

namespace Cine.Modules.Sales.Domain.DiscountPolicies;

internal sealed class DiscountPolicy(
    ISpecification<ReservationContext> specification,
    DiscountStrategy strategy)
{
    public void ApplyTo(ReservationContext reservationContext)
    {
        if (specification.IsSatisfiedBy(reservationContext))
        {
            reservationContext.ReduceAmount(strategy.Calculate(reservationContext));
        }
    }
}
