using Cine.Shared.Domain.Specifications;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class AndSpecification(params ISpecification<ReservationContext>[] specifications)
    : ISpecification<ReservationContext>
{
    public bool IsSatisfiedBy(ReservationContext other) =>
        specifications.All(specification => specification.IsSatisfiedBy(other));
}

internal sealed class NotSpecification(ISpecification<ReservationContext> specification)
    : ISpecification<ReservationContext>
{
    public bool IsSatisfiedBy(ReservationContext reservationContext) =>
        !specification.IsSatisfiedBy(reservationContext);
}

internal class DayOfWeekSpecification(
    DayOfWeek dayOfWeek,
    Func<ReservationContext, DateTime> date)
    : ISpecification<ReservationContext>
{
    public bool IsSatisfiedBy(ReservationContext reservationContext)
    {
        var gimmeDayOfWeek = date(reservationContext).DayOfWeek;
        var equalsOrNotEquals = gimmeDayOfWeek.Equals(dayOfWeek);
        return equalsOrNotEquals;
    }
}