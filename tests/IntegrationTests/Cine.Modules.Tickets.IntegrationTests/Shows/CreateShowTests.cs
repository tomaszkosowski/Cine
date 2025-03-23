using Cine.Modules.Tickets.Application.Shows.CreateShow;
using FluentAssertions;

namespace Cine.Modules.Tickets.IntegrationTests.Shows;

public class CreateShowTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task CreateShow_WhenValidCommand_ShouldReturnShowId()
    {
        // Arrange
        var showId = Guid.NewGuid();
        var hallId = Guid.NewGuid();

        var command = new CreateShowCommand(showId, hallId, DateTime.Parse("2024-01-30T12:00:00"));

        // Act
        var result = await Sender.Send(command);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().Be(showId);
    }
}