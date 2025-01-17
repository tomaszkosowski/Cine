using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountRules;

/// <summary>
/// Special offer for Tue-Thu mornings.
/// </summary>
internal class BreakfastInAmericaDiscountRule : DiscountRule
{
    private readonly DiscountSpecification _specification =
        new MinimumAmountDiscountSpecification(8.0,
            new AndSpecification(
                new NotSpecification(new DayOfWeekSpecification(DayOfWeek.Monday, StartAt, Empty)),
                new NotSpecification(new DayOfWeekSpecification(DayOfWeek.Friday, StartAt, Empty)),
                new NotSpecification(new DayOfWeekSpecification(DayOfWeek.Saturday, StartAt, Empty)),
                new NotSpecification(new DayOfWeekSpecification(DayOfWeek.Sunday, StartAt, Empty)),
                new MorningDiscountSpecification(new FixedAmountDiscountStrategy(2.0))));

    private static EmptyDiscountStrategy Empty => new();

    private static Func<ReservationContext, DateTime> StartAt => reservationContext => reservationContext.StartAt;

    public override double ApplyDiscounts(ReservationContext reservationContext)
    {
        _specification.ApplyTo(reservationContext);

        return reservationContext.Amount;
    }
}