using Cine.Shared.Domain.Specifications;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

/// <summary>
/// Guards discount application by requiring the current amount to meet the minimum threshold.
/// </summary>
internal sealed class MinimumAmountSpecification(double minimumAmount)
    : ISpecification<ReservationContext>
{
    public bool IsSatisfiedBy(ReservationContext reservationContext) =>
        reservationContext.Amount >= minimumAmount;
}