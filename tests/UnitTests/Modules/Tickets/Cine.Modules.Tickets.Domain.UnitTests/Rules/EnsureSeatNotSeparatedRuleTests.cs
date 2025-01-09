using Cine.Modules.Tickets.Domain.Rules;
using Cine.Modules.Tickets.Domain.UnitTests.Factories;
using FluentAssertions;

namespace Cine.Modules.Tickets.Domain.UnitTests.Rules;

public class EnsureSeatNotSeparatedRuleTests
{
    [Fact]
    public void IsBroken_WhenNoOtherSeatsReserved_ShouldReturnFalse()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject("I", 2);

        var rule = new EnsureSeatNotSeparatedRule(seat, []);

        // Act
        var result = rule.IsBroken();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsBroken_WhenDifferentRowSeatIsReserved_ShouldReturnFalse()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject("I", 2);

        var rule = new EnsureSeatNotSeparatedRule(seat, [SeatObjectFactory.CreateValidObject("II", 2)]);

        // Act
        var result = rule.IsBroken();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void IsBroken_WhenSeatIsAdjacent_ShouldReturnFalse(int number)
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject("I", number);

        var rule = new EnsureSeatNotSeparatedRule(seat,
        [
            SeatObjectFactory.CreateValidObject("I", 2),
            SeatObjectFactory.CreateValidObject("I", 4)
        ]);

        // Act
        var result = rule.IsBroken();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsBroken_WhenSeatIsNotAdjacent_ShouldReturnTrue()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject("I", 5);

        var rule = new EnsureSeatNotSeparatedRule(seat,
        [
            SeatObjectFactory.CreateValidObject("I", 1),
            SeatObjectFactory.CreateValidObject("I", 3)
        ]);

        // Act
        var result = rule.IsBroken();

        // Assert
        result.Should().BeTrue();
    }
}