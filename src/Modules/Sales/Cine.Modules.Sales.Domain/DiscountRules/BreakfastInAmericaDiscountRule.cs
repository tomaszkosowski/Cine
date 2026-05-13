using Cine.Modules.Sales.Domain.DiscountPolicies;
using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountRules;

/// <summary>
/// Special offer for Tue-Thu mornings.
/// </summary>
public sealed class BreakfastInAmericaDiscountRule : DiscountRule
{
    private protected override IReadOnlyList<DiscountPolicy> Policies { get; } =
    [
        new DiscountPolicy(
            new AndSpecification(
                new MinimumAmountSpecification(8.0),
                new AllowedDaysSpecification(DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday),
                new MorningSpecification()),
            new FixedAmountDiscountStrategy(2.0))
    ];
}