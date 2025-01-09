using Cine.Modules.Tickets.Domain.Events;
using Cine.Modules.Tickets.Domain.Rules;
using Cine.Modules.Tickets.Domain.UnitTests.Factories;
using Cine.Shared.Domain.UnitTests;
using FluentAssertions;

namespace Cine.Modules.Tickets.Domain.UnitTests;

public class SeatTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateOpenSeat()
    {
        // Arrange
        var createSeat = () => SeatObjectFactory.CreateValidObject();

        // Act
        var seat = createSeat();

        // Assert
        seat.Status.Should().Be(SeatStatusType.Open);
    }

    [Fact]
    public void ChangeStatus_OpenToReserved_ShouldPublishSeatReservedDomainEvent()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject();

        // Act
        seat.ChangeStatus(SeatStatusType.Reserved);

        // Assert
        seat.Status.Should().Be(SeatStatusType.Reserved);

        var domainEvent = seat.GetDomainEvent<SeatReservedDomainEvent>();
        domainEvent.Should().NotBeNull();
    }

    [Fact]
    public void ChangeStatus_OpenToPurchased_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject();

        // Act
        var action = () => seat.ChangeStatus(SeatStatusType.Purchased);

        // Assert
        action.AssertBrokenRule<EnsureSeatNotOpenedRule>();
    }

    [Fact]
    public void ChangeStatus_ReservedToOpen_ShouldPublishSeatReleasedDomainEvent()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject();
        seat.ChangeStatus(SeatStatusType.Reserved);

        // Act
        seat.ChangeStatus(SeatStatusType.Open);

        // Assert
        seat.Status.Should().Be(SeatStatusType.Open);

        var domainEvent = seat.GetDomainEvent<SeatReleasedDomainEvent>();
        domainEvent.Should().NotBeNull();
    }

    [Fact]
    public void ChangeStatus_ReservedToPurchased_ShouldPublishSeatReleasedDomainEvent()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject();
        seat.ChangeStatus(SeatStatusType.Reserved);

        // Act
        seat.ChangeStatus(SeatStatusType.Purchased);

        // Assert
        seat.Status.Should().Be(SeatStatusType.Purchased);

        var domainEvent = seat.GetDomainEvent<SeatPurchasedDomainEvent>();
        domainEvent.Should().NotBeNull();
    }

    [Fact]
    public void ChangeStatus_PurchasedToOpen_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject();
        seat.ChangeStatus(SeatStatusType.Reserved);
        seat.ChangeStatus(SeatStatusType.Purchased);

        // Act
        var action = () => seat.ChangeStatus(SeatStatusType.Open);

        // Assert
        action.AssertBrokenRule<EnsureSeatNotPurchasedRule>();
    }
    
    [Fact]
    public void ChangeStatus_PurchasedToReserved_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var seat = SeatObjectFactory.CreateValidObject();
        seat.ChangeStatus(SeatStatusType.Reserved);
        seat.ChangeStatus(SeatStatusType.Purchased);

        // Act
        var action = () => seat.ChangeStatus(SeatStatusType.Reserved);

        // Assert
        action.AssertBrokenRule<EnsureSeatNotPurchasedRule>();
    }
}