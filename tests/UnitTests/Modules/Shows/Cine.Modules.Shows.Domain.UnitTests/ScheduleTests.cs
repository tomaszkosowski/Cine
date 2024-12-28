using Cine.Shared.Domain;
using Cine.Shared.Domain.Rules;
using Cine.Shared.Domain.UnitTests;
using FluentAssertions;

namespace Cine.Modules.Shows.Domain.UnitTests;

public class ScheduleTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateValidObject()
    {
        // Arrange
        Utc.Override(new DateTime(2024, 01, 01));

        var scheduledAt = DateTime.Parse("2024-01-30T12:00:00");
        var duration = TimeSpan.FromMinutes(90);

        // Act
        var schedule = Schedule.Create(scheduledAt, duration);

        // Assert
        schedule.EndAt.Should().Be(DateTime.Parse("2024-01-30T13:30:00"));

        Utc.Rollback();
    }

    [Fact]
    public void Create_WithNegativeDuration_ShouldThrowBusinessRuleException()
    {
        // Arrange
        Utc.Override(new DateTime(2024, 01, 01));

        var scheduledAt = DateTime.Parse("2024-01-30T12:00:00");
        var duration = TimeSpan.FromMinutes(-90);

        // Act
        var createSchedule = () => Schedule.Create(scheduledAt, duration);

        // Assert
        createSchedule.AssertBrokenRule<EnsureNotNegativeRule>();

        Utc.Rollback();
    }

    [Fact]
    public void Create_WithZeroDuration_ShouldThrowBusinessRuleException()
    {
        // Arrange
        Utc.Override(new DateTime(2024, 01, 01));

        var scheduledAt = DateTime.Parse("2024-01-30T12:00:00");
        var duration = TimeSpan.FromMinutes(0);

        // Act
        var createSchedule = () => Schedule.Create(scheduledAt, duration);

        // Assert
        createSchedule.AssertBrokenRule<EnsureNotZeroRule>();

        Utc.Rollback();
    }

    [Fact]
    public void Create_WithPastScheduledAt_ShouldThrowBusinessRuleException()
    {
        // Arrange
        Utc.Override(new DateTime(2024, 01, 30));

        var scheduledAt = DateTime.Parse("2022-01-30T12:00:00");
        var duration = TimeSpan.FromMinutes(90);

        // Act
        var createSchedule = () => Schedule.Create(scheduledAt, duration);

        // Assert
        createSchedule.AssertBrokenRule<EnsureNotPastRule>();

        Utc.Rollback();
    }

    [Fact]
    public void IsOverlapping_WithOverlappingSchedule_ShouldReturnTrue()
    {
        // Arrange
        Utc.Override(new DateTime(2024, 01, 30));

        var other = Schedule.Create(DateTime.Parse("2024-01-30T12:00:00"), TimeSpan.FromMinutes(90));
        var schedule = Schedule.Create(DateTime.Parse("2024-01-30T13:00:00"), TimeSpan.FromMinutes(90));

        // Act
        var result = schedule.IsOverlapping(other);

        // Assert
        result.Should().BeTrue();

        Utc.Rollback();
    }
}