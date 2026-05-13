namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

internal sealed class MondaySpecification()
    : DayOfWeekSpecification(DayOfWeek.Monday, reservationContext => reservationContext.StartAt);