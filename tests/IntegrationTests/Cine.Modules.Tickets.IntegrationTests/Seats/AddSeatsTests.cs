using Cine.Modules.Tickets.Application.ApiClients.Theater;
using Cine.Modules.Tickets.Application.Seats;
using FluentAssertions;
using NSubstitute;

namespace Cine.Modules.Tickets.IntegrationTests.Seats;

public class AddSeatsTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task AddSeats_WhenValidCommand_ShouldReturnSeatIds()
    {
        // Arrange
        var hallId = Guid.NewGuid();
        var showId = Guid.NewGuid();

        var command = new AddSeatsCommand(hallId, showId);
        TheaterApiClient.GetSeatsAsync(hallId).Returns(
            new SeatsDto([new SeatDto(Guid.NewGuid(), "I", 1)]));

        // Act
        var result = await Sender.Send(command, TestContext.Current.CancellationToken);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().NotBeNullOrEmpty();
    }
}