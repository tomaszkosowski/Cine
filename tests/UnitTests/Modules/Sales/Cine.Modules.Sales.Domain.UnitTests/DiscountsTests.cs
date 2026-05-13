using Cine.Modules.Sales.Domain.DiscountRules;
using Cine.Modules.Sales.Domain.DiscountPolicies;
using Cine.Modules.Sales.Domain.DiscountSpecifications;
using Cine.Modules.Sales.Domain.DiscountStrategies;
using FluentAssertions;

namespace Cine.Modules.Sales.Domain.UnitTests;

public class DiscountsTests
{
    [Fact]
    public void DiscountPolicy_WhenSpecificationIsSatisfied_ShouldApplyFixedAmountDiscount()
    {
        var reservationContext = ReservationContext.Create(12.0, new DateTime(2025, 01, 13), 4);
        var policy = new DiscountPolicy(new MondaySpecification(), new FixedAmountDiscountStrategy(1.0));
        
        policy.ApplyTo(reservationContext);

        reservationContext.Amount.Should().Be(11.0);
    }

    [Fact]
    public void DiscountPolicy_WhenSpecificationIsNotSatisfied_ShouldNotChangeAmount()
    {
        var reservationContext = ReservationContext.Create(12.0, new DateTime(2025, 01, 14), 4);
        var policy = new DiscountPolicy(new MondaySpecification(), new PercentageDiscountStrategy(50.0));
        
        policy.ApplyTo(reservationContext);

        reservationContext.Amount.Should().Be(12.0);
    }

    [Fact]
    public void AndSpecification_WhenAllPredicatesSatisfied_ShouldReturnTrue()
    {
        var reservationContext = ReservationContext.Create(12.0, new DateTime(2025, 01, 13), 5);
        var specification = new AndSpecification(new MondaySpecification(), new GroupSpecification());
        
        specification.IsSatisfiedBy(reservationContext).Should().BeTrue();
    }

    [Fact]
    public void AllowedDaysSpecification_WhenDayIsNotAllowed_ShouldReturnFalse()
    {
        var reservationContext = ReservationContext.Create(10.0, new DateTime(2025, 01, 17, 8, 15, 0), 2);
        var specification = new AllowedDaysSpecification(DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday);
        
        specification.IsSatisfiedBy(reservationContext).Should().BeFalse();
    }

    [Fact]
    public void AllowedDaysSpecification_WhenDayIsAllowed_ShouldReturnTrue()
    {
        var reservationContext = ReservationContext.Create(10.0, new DateTime(2025, 01, 16, 8, 15, 0), 2);
        var specification = new AllowedDaysSpecification(DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday);
        
        specification.IsSatisfiedBy(reservationContext).Should().BeTrue();
    }

    [Fact]
    public void NotSpecification_WhenInnerPredicateIsSatisfied_ShouldReturnFalse()
    {
        var reservationContext = ReservationContext.Create(10.0, new DateTime(2025, 01, 13), 2);
        var specification = new NotSpecification(new MondaySpecification());
        
        specification.IsSatisfiedBy(reservationContext).Should().BeFalse();
    }

    [Fact]
    public void NotSpecification_WhenInnerPredicateIsNotSatisfied_ShouldReturnTrue()
    {
        var reservationContext = ReservationContext.Create(10.0, new DateTime(2025, 01, 14), 2);
        var specification = new NotSpecification(new MondaySpecification());
        
        specification.IsSatisfiedBy(reservationContext).Should().BeTrue();
    }

    [Fact]
    public void MondaySpecialDiscountRule_WhenAllConditionsAreMet_ShouldApplyPercentageDiscount()
    {
        var reservationContext = ReservationContext.Create(80.0, new DateTime(2025, 01, 13), 5);

        var mondaySpecial = new MondaySpecialDiscountRule();
        var amount = mondaySpecial.ApplyDiscounts(reservationContext);

        amount.Should().Be(72.0);
    }

    [Fact]
    public void BreakfastInAmericaDiscountRule_WhenDateIsFridayMorning_ShouldNotApplyDiscount()
    {
        var reservationContext = ReservationContext.Create(10.0, new DateTime(2025, 01, 17, 8, 15, 0), 2);

        var breakfastInAmerica = new BreakfastInAmericaDiscountRule();
        var amount = breakfastInAmerica.ApplyDiscounts(reservationContext);

        amount.Should().Be(10);
    }

    [Fact]
    public void BreakfastInAmericaDiscountRule_WhenDateIsEligibleMorning_ShouldApplyFixedDiscount()
    {
        var reservationContext = ReservationContext.Create(10.0, new DateTime(2025, 01, 16, 8, 15, 0), 2);

        var breakfastInAmerica = new BreakfastInAmericaDiscountRule();
        var amount = breakfastInAmerica.ApplyDiscounts(reservationContext);

        amount.Should().Be(8);
    }
}