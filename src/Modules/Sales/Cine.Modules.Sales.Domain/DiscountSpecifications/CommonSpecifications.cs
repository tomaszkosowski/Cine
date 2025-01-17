using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class AndSpecification(params IEnumerable<DiscountSpecification> specifications)
    : DiscountSpecification(new EmptyDiscountStrategy())
{
    public override bool IsSatisfiedBy(ReservationContext other) =>
        specifications.All(specification => specification.IsSatisfiedBy(other));

    public override void ApplyTo(ReservationContext reservationContext)
    {
        if (!IsSatisfiedBy(reservationContext))
        {
            return;
        }

        foreach (var specification in specifications)
        {
            specification.ApplyTo(reservationContext);
        }
    }
}

internal sealed class NotSpecification(DiscountSpecification specification)
    : DiscountSpecification(new EmptyDiscountStrategy())
{
    public override bool IsSatisfiedBy(ReservationContext reservationContext) =>
        !specification.IsSatisfiedBy(reservationContext);

    public override void ApplyTo(ReservationContext reservationContext)
    {
        if (!IsSatisfiedBy(reservationContext))
        {
            return;
        }

        specification.ApplyTo(reservationContext);
    }
}

internal class DayOfWeekSpecification(
    DayOfWeek dayOfWeek,
    Func<ReservationContext, DateTime> date,
    DiscountStrategy discountStrategy)
    : DiscountSpecification(discountStrategy)
{
    public override bool IsSatisfiedBy(ReservationContext reservationContext)
    {
        var gimmeDayOfWeek = date(reservationContext).DayOfWeek;
        var equalsOrNotEquals = gimmeDayOfWeek.Equals(dayOfWeek);
        return equalsOrNotEquals;
    }
}