using Cine.Shared.Domain.Specifications;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class GroupSpecification
    : ISpecification<ReservationContext>
{
    public bool IsSatisfiedBy(ReservationContext reservationContext)
        => reservationContext.SeatsCount > 4;
}