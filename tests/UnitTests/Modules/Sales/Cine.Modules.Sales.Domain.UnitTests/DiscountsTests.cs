using Cine.Modules.Sales.Domain.DiscountRules;
using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.DiscountStrategies;
using FluentAssertions;

namespace Cine.Modules.Sales.Domain.UnitTests;

public class DiscountsTests
{
    [Fact]
    public void Sample1()
    {
        var reservationContext = ReservationContext.Create(12.0, new DateTime(2025, 01, 13), 4);

        var fixedAmount = new FixedAmountDiscountStrategy(1.0);

        var mondayDiscount = new MondayDiscountSpecification(fixedAmount);

        mondayDiscount.ApplyTo(reservationContext);

        reservationContext.Amount.Should().Be(11.0);
    }

    [Fact]
    public void Sample2()
    {
        var reservationContext = ReservationContext.Create(12.0, new DateTime(2025, 01, 13), 4);

        var percentage = new PercentageDiscountStrategy(50.0);

        var mondayDiscount = new MondayDiscountSpecification(percentage);

        mondayDiscount.ApplyTo(reservationContext);

        reservationContext.Amount.Should().Be(6.0);
    }

    [Fact]
    public void Sample3()
    {
        var reservationContext = ReservationContext.Create(12.0, new DateTime(2025, 01, 13), 5);

        var fixedAmount = new FixedAmountDiscountStrategy(2.0);
        var percentage = new PercentageDiscountStrategy(50.0);

        var monday = new MondayDiscountSpecification(fixedAmount);
        var group = new GroupDiscountSpecification(percentage);

        var and = new AndSpecification(monday, group);
        and.ApplyTo(reservationContext);

        reservationContext.Amount.Should().Be(5.0);
    }

    [Fact]
    public void Sample4()
    {
        var reservationContext = ReservationContext.Create(80.0, new DateTime(2025, 01, 13), 5);

        var mondaySpecial = new MondaySpecialDiscountRule();
        var amount = mondaySpecial.ApplyDiscounts(reservationContext);

        amount.Should().Be(72.0);
    }

    [Fact]
    public void Sample5()
    {
        var reservationContext = ReservationContext.Create(10.0, new DateTime(2025, 01, 17, 8, 15, 0), 2);

        var breakfastInAmerica = new BreakfastInAmericaDiscountRule();
        var amount = breakfastInAmerica.ApplyDiscounts(reservationContext);

        amount.Should().Be(10);
    }
}