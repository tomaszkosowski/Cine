using Cine.Modules.Tickets.Domain.Events;
using Cine.Modules.Tickets.Domain.Rules;
using Cine.Modules.Tickets.Domain.UnitTests.Factories;
using Cine.Shared.Domain.UnitTests;
using FluentAssertions;

namespace Cine.Modules.Tickets.Domain.UnitTests;

public class ReservationTests
{
    [Fact]
    public void Create_WithValidData_ShouldPublishReservationCreatedDomainEvent()
    {
        // Arrange
        var createReservation = ReservationObjectFactory.CreateValidObject;

        // Act
        var reservation = createReservation();

        // Assert 
        reservation.Seats.Should().BeEmpty();
        reservation.ReservationStatus.Should().BeOfType<Unpaid>();

        var domainEvent = reservation.GetDomainEvent<ReservationCreatedDomainEvent>();
        domainEvent.Should().NotBeNull();
    }

    [Fact]
    public void AddSeat_WithValidData_ShouldAddSeat()
    {
        // Arrange
        var reservation = ReservationObjectFactory.CreateValidObject();
        var seat = SeatObjectFactory.CreateValidObject();

        // Act
        reservation.AddSeat(seat);

        // Assert
        reservation.Seats.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveSeat_WithValidData_ShouldRemoveSeat()
    {
        // Arrange
        var reservation = ReservationObjectFactory.CreateValidObject();
        var seat = SeatObjectFactory.CreateValidObject();

        reservation.AddSeat(seat);

        // Act
        reservation.RemoveSeat(seat);

        // Assert
        reservation.Seats.Should().HaveCount(0);
    }

    [Fact]
    public void Confirm_WhenReservationEmpty_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var reservation = ReservationObjectFactory.CreateValidObject();

        // Act
        var confirmReservation = () => reservation.Confirm();

        // Assert
        reservation.Seats.Should().BeEmpty();
        confirmReservation.AssertBrokenRule<EnsureReservationNotEmpty>();
    }

    [Fact]
    public void Expire_WhenUnpaid_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var reservation = ReservationObjectFactory.CreateValidObject();

        // Act
        reservation.Expire();

        // Assert
        reservation.ReservationStatus.Should().BeOfType<Expired>();

        var domainEvent = reservation.GetDomainEvent<ReservationExpiredDomainEvent>();
        domainEvent.Should().NotBeNull();
    }

    [Fact]
    public void Expire_WhenConfirmed_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var reservation = ReservationObjectFactory.CreateValidObject();
        reservation.AddSeat(SeatObjectFactory.CreateValidObject());
        reservation.Confirm();

        // Act
        var expire = () => reservation.Expire();

        // Assert
        expire.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Cannot advance from Confirmed to Expired");
    }
}