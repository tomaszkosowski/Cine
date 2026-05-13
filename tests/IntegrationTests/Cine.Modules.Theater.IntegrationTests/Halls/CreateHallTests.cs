using Cine.Modules.Theater.Application.Halls.CreateHall;
using FluentAssertions;
using Snapshooter.Xunit3;

namespace Cine.Modules.Theater.IntegrationTests.Halls;

public class CreateHallTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task CreateHall_WhenValidCommand_ShouldReturnHallIdAndSeatDtos()
    {
        // Arrange
        var command = new CreateHallCommand("Hall#1", (3, 5));

        // Act
        var result = await Sender.Send(command, TestContext.Current.CancellationToken);

        // Assert
        result.IsT0.Should().BeTrue();
        Snapshot.Match(result.AsT0, opts => opts.IgnoreField("Item1"));
    }
}