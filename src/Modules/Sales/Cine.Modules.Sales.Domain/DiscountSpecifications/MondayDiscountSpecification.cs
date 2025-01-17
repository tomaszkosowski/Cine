using Cine.Modules.Sales.Domain.DiscountStrategies;

namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class MondayDiscountSpecification(DiscountStrategy discountStrategy)
    : DayOfWeekSpecification(DayOfWeek.Monday, reservationContext => reservationContext.StartAt, discountStrategy);