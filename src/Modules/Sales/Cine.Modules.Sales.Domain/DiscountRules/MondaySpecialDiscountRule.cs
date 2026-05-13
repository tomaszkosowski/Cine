using Cine.Modules.Sales.Domain.DiscountPolicies;
using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountRules;

/// <summary>
/// Special offer on Mondays for groups.
/// </summary>
public sealed class MondaySpecialDiscountRule : DiscountRule
{
    private protected override IReadOnlyList<DiscountPolicy> Policies { get; } =
    [
        new DiscountPolicy(
            new AndSpecification(
                new MinimumAmountSpecification(40.0),
                new MondaySpecification(),
                new GroupSpecification()),
            new PercentageDiscountStrategy(10.0))
    ];
}