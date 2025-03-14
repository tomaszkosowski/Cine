using Cine.Modules.Shows.Application.Halls.AddHall;
using Cine.Modules.Shows.Application.Halls.GetHall;
using Cine.Modules.Shows.Application.Movies.AddMovie;
using Cine.Modules.Shows.Application.Movies.GetMovie;
using FluentAssertions;
using MediatR;
using OneOf.Types;
using Snapshooter.Xunit;

namespace Cine.Modules.Shows.IntegrationTests.Movies;

public class GetMovieTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task GetMovie_WhenMovieExists_ShouldReturnMovieDto()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        await AddMovieAsync(movieId);
        
        var query = new GetMovieQuery(movieId);

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.MovieId.Should().Be(movieId);
        Snapshot.Match(result.AsT0, opts => opts.IgnoreField("MovieId"));
    }
    
    [Fact]
    public async Task GetHall_WhenMovieNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        
        var query = new GetMovieQuery(movieId);

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Should().BeOfType<NotFound>();
    }

    private async Task AddMovieAsync(Guid movieId)
    {
        var command = new AddMovieCommand(movieId, TimeSpan.Parse("1:30:00"));
        var result = await Sender.Send(command);

        result.Should().Be(Unit.Value);
    }
}