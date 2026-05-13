using Cine.Modules.Shows.Application.Movies.AddMovie;
using FluentAssertions;
using MediatR;

namespace Cine.Modules.Shows.IntegrationTests.Movies;

public class AddMovieTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task AddMovie_WhenValidCommand_ShouldReturnUnitValue()
    {
        // Arrange
        var command = new AddMovieCommand(Guid.NewGuid(), TimeSpan.Parse("1:30:00"));

        // Act
        var result = await Sender.Send(command, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(Unit.Value);
    }
}