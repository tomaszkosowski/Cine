using Cine.Modules.Sales.Domain.DiscountStrategies;
using Cine.Shared.Domain.Specifications;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal abstract class DiscountSpecification(DiscountStrategy discountStrategy) : ISpecification<ReservationContext>
{
    public abstract bool IsSatisfiedBy(ReservationContext reservationContext);

    public virtual void ApplyTo(ReservationContext reservationContext) => reservationContext.ReduceAmount(
        IsSatisfiedBy(reservationContext)
            ? discountStrategy.Calculate(reservationContext)
            : 0.0);
}