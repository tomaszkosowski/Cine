using Cine.Modules.Theater.Application.Halls.CreateHall;
using Cine.Modules.Theater.Application.Seats.GetSeats;
using FluentAssertions;
using Snapshooter.Xunit;

namespace Cine.Modules.Theater.IntegrationTests.Seats;

public class GetSeatsTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task GetSeats_WhenValidQuery_ShouldReturnSeatDtos()
    {
        // Arrange
        var hallId = await CreateHallAsync();
        var query = new GetSeatsQuery(hallId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        result.IsT0.Should().BeTrue();
        Snapshot.Match(result.AsT0, opts =>
        {
            opts.IgnoreFields("[*].SeatId");
            opts.IgnoreFields("[*].HallId");

            return opts;
        });
    }

    private async Task<Guid> CreateHallAsync()
    {
        var command = new CreateHallCommand("Hall#1", (3, 5));
        var result = await Sender.Send(command);

        result.IsT0.Should().BeTrue();

        return result.AsT0.HallId;
    }
}