using Cine.Modules.Tickets.Application.Reservations.CreateReservation;
using Cine.Modules.Tickets.Application.Shows.CreateShow;
using FluentAssertions;

namespace Cine.Modules.Tickets.IntegrationTests.Reservations;

public class CreateReservationTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task CreateReservation_WhenValidCommand_ShouldReturnReservationId()
    {
        // Arrange
        var showId = Guid.NewGuid();
        
        await AddShowAsync(showId);

        var command = new CreateReservationCommand(showId);

        // Act
        var result = await Sender.Send(command);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().NotBeEmpty();
    }

    private async Task AddShowAsync(Guid showId)
    {
        var command = new CreateShowCommand(showId, Guid.NewGuid(), DateTime.Parse("2024-01-30T12:00:00"));
        
        var result = await Sender.Send(command);

        result.IsT0.Should().BeTrue();
    }
}