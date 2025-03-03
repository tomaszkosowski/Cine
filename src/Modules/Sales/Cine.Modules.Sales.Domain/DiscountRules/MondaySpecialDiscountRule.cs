using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountRules;

/// <summary>
/// Special offer on Mondays for groups.
/// </summary>
public sealed class MondaySpecialDiscountRule : DiscountRule
{
    private readonly DiscountSpecification _specification =
        new MinimumAmountDiscountSpecification(40.0,
            new AndSpecification(
                new MondayDiscountSpecification(new EmptyDiscountStrategy()),
                new GroupDiscountSpecification(new PercentageDiscountStrategy(10.0))));

    public override double ApplyDiscounts(ReservationContext reservationContext)
    {
        _specification.ApplyTo(reservationContext);

        return reservationContext.Amount;
    }
}