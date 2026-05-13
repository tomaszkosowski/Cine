using Cine.Modules.Sales.Domain.DiscountPolicies;
using Cine.Modules.Sales.Domain.DiscountSpecifications;

namespace Cine.Modules.Sales.Domain.DiscountRules;

public abstract class DiscountRule
{
    private protected abstract IReadOnlyList<DiscountPolicy> Policies { get; }

    public double ApplyDiscounts(ReservationContext reservationContext)
    {
        foreach (var policy in Policies)
        {
            policy.ApplyTo(reservationContext);
        }

        return reservationContext.Amount;
    }
}