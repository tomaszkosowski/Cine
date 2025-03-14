using Cine.Modules.Shows.Application.Halls.AddHall;
using Cine.Modules.Shows.Application.Halls.GetHall;
using FluentAssertions;
using MediatR;
using OneOf.Types;

namespace Cine.Modules.Shows.IntegrationTests.Halls;

public class GetHallTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task GetHall_WhenHallExists_ShouldReturnHallDto()
    {
        // Arrange
        var hallId = Guid.NewGuid();
        await AddHallAsync(hallId);
        
        var query = new GetHallQuery(hallId);

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.HallId.Should().Be(hallId);
    }
    
    [Fact]
    public async Task GetHall_WhenHallNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var hallId = Guid.NewGuid();
        
        var query = new GetHallQuery(hallId);

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Should().BeOfType<NotFound>();
    }

    private async Task AddHallAsync(Guid hallId)
    {
        var command = new AddHallCommand(hallId);
        var result = await Sender.Send(command);

        result.Should().Be(Unit.Value);
    }
}