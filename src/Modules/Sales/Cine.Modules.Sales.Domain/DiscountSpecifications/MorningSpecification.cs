using Cine.Shared.Domain.Specifications;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class MorningSpecification
    : ISpecification<ReservationContext>
{
    public bool IsSatisfiedBy(ReservationContext reservationContext) =>
        reservationContext.StartAt.Hour is >= 8 and < 12;
}