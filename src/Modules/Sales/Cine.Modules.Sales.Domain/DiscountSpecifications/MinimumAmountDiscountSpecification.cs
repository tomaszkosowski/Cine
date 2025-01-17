using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class MinimumAmountDiscountSpecification(
    double minimumAmount,
    params IEnumerable<DiscountSpecification> specifications)
    : DiscountSpecification(new EmptyDiscountStrategy())
{
    public override bool IsSatisfiedBy(ReservationContext reservationContext) =>
        reservationContext.Amount >= minimumAmount;

    public override void ApplyTo(ReservationContext reservationContext)
    {
        foreach (var specification in specifications)
        {
            if (TryApplyTo(specification))
            {
                specification.ApplyTo(reservationContext);
            }
        }

        return;

        bool TryApplyTo(DiscountSpecification specification)
        {
            var transactionContext = reservationContext.Clone();

            specification.ApplyTo(transactionContext);
            return IsSatisfiedBy(transactionContext);
        }
    }
}