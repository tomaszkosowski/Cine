using Cine.Modules.Tickets.Domain.Rules;
using Cine.Modules.Tickets.Domain.UnitTests.Factories;
using FluentAssertions;

namespace Cine.Modules.Tickets.Domain.UnitTests.Rules;

public class EnsureSeatNotAdjacentRuleTests
{
    [Fact]
    public void IsBroken_WhenAnyLeftNotAdjacent_ShouldReturnTrue()
    {
        // Arrange
        var seat1 = SeatObjectFactory.CreateValidObject("I", 1);
        var seat2 = SeatObjectFactory.CreateValidObject("I", 2);
        var seat3 = SeatObjectFactory.CreateValidObject("I", 3);
        var seat4 = SeatObjectFactory.CreateValidObject("I", 4);

        var rule = new EnsureSeatNotAdjacentRule(seat2, [seat1, seat2, seat3, seat4]);

        // Act
        var result = rule.IsBroken();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsBroken_WhenOneLeft_ShouldReturnFalse()
    {
        // Arrange
        var seat1 = SeatObjectFactory.CreateValidObject("I", 1);
        var seat2 = SeatObjectFactory.CreateValidObject("I", 2);

        var rule = new EnsureSeatNotAdjacentRule(seat2, [seat1, seat2]);

        // Act
        var result = rule.IsBroken();

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void IsBroken_WhenNoneLeft_ShouldReturnFalse()
    {
        // Arrange
        var seat2 = SeatObjectFactory.CreateValidObject("I", 2);

        var rule = new EnsureSeatNotAdjacentRule(seat2, [seat2]);

        // Act
        var result = rule.IsBroken();

        // Assert
        result.Should().BeFalse();
    }
}