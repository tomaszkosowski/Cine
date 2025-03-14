using Cine.Modules.Shows.Application.Halls.AddHall;
using FluentAssertions;
using MediatR;

namespace Cine.Modules.Shows.IntegrationTests.Halls;

public class AddHallTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task AddHall_WhenValidCommand_ShouldReturnUnitValue()
    {
        // Arrange
        var command = new AddHallCommand(Guid.NewGuid());

        // Act
        var result = await Sender.Send(command);

        // Assert
        result.Should().Be(Unit.Value);
    }
}