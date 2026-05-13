using Cine.Shared.Domain.Specifications;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class AllowedDaysSpecification(params DayOfWeek[] allowedDays)
    : ISpecification<ReservationContext>
{
    public bool IsSatisfiedBy(ReservationContext reservationContext) =>
        allowedDays.Contains(reservationContext.StartAt.DayOfWeek);
}
