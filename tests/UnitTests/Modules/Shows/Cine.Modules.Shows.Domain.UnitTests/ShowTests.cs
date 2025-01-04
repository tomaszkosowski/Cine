using Cine.Modules.Shows.Domain.Events;
using Cine.Modules.Shows.Domain.Rules;
using Cine.Shared.Domain;
using Cine.Shared.Domain.UnitTests;
using FluentAssertions;

namespace Cine.Modules.Shows.Domain.UnitTests;

public class ShowTests
{
    [Fact]
    public void Create_WithValidData_ShouldPublishShowCreatedDomainEvent()
    {
        // Arrange
        Utc.Override(new DateTime(2024, 01, 01));

        var hallId = HallId.Create();
        var movieId = MovieId.Create();
        var schedule = Schedule.Create(DateTime.Parse("2024-01-30T12:00:00"), TimeSpan.FromMinutes(90));

        var createShow = () => Show.Create(hallId, movieId, schedule, []);

        // Act
        var show = createShow();

        // Assert
        var domainEvent = show.GetDomainEvent<ShowCreatedDomainEvent>();

        domainEvent.Should().NotBeNull();
        domainEvent?.ShowId.Should().Be(show.ShowId);

        Utc.Rollback();
    }

    [Fact]
    public void Create_WithOverlappedShow_ShouldThrowBusinessRuleException()
    {
        // Arrange
        Utc.Override(new DateTime(2024, 01, 01));

        var hallId = HallId.Create();
        var movieId = MovieId.Create();
        var schedule = Schedule.Create(DateTime.Parse("2024-01-30T12:00:00"), TimeSpan.FromMinutes(90));
        var otherShow = new ShowInfo(hallId,
            Schedule.Create(DateTime.Parse("2024-01-30T10:31:00"), TimeSpan.FromMinutes(90)));

        // Act
        var createShow = () => Show.Create(hallId, movieId, schedule, [otherShow]);

        // Assert
        createShow.AssertBrokenRule<EnsureNotOverlapsOtherShows>();

        Utc.Rollback();
    }
}