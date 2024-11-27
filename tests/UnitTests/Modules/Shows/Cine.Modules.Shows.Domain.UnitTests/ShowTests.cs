using Cine.Modules.Shows.Domain.Events;
using Cine.Shared.Domain;
using Cine.Shared.Domain.UnitTests;
using FluentAssertions;

namespace Cine.Modules.Shows.Domain.UnitTests
{
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
    }
}
