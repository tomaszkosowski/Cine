using Cine.Modules.Shows.Application.Halls.AddHall;
using Cine.Modules.Shows.Application.Movies.AddMovie;
using Cine.Modules.Shows.Application.Shows.CreateShow;
using Cine.Shared.Domain;
using FluentAssertions;
using MediatR;

namespace Cine.Modules.Shows.IntegrationTests.Shows;

public class CreateShowTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task CreateShow_WhenValidCommand_ShouldReturnShowId()
    {
        // Arrange
        Utc.Override(DateTime.Parse("2025-03-01T00:00:00"));
        
        var hallId = Guid.NewGuid();
        await AddHallAsync(hallId);
        
        var movieId = Guid.NewGuid();
        await AddMovieAsync(movieId);

        var command = new CreateShowCommand(hallId, movieId, DateTime.Parse("2025-03-01T12:00:00"));

        // Act
        var result = await Sender.Send(command, TestContext.Current.CancellationToken);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().NotBeEmpty();
        
        Utc.Rollback();
    }

    private async Task AddHallAsync(Guid hallId)
    {
        var command = new AddHallCommand(hallId);
        var result = await Sender.Send(command);
        
        result.Should().Be(Unit.Value);
    }
    
    private async Task AddMovieAsync(Guid movieId)
    {
        var command = new AddMovieCommand(movieId, TimeSpan.Parse("1:30:00"));
        var result = await Sender.Send(command);
        
        result.Should().Be(Unit.Value);
    }
}