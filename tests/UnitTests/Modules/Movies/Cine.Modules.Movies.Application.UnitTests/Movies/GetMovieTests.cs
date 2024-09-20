using Cine.Modules.Movies.Application.Movies.GetMovie;
using Cine.Shared.Application.Database;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Cine.Modules.Movies.Application.UnitTests.Movies
{
    public class GetMovieTests
    {
        private readonly ISqlConnection _sqlConnection = Substitute.For<ISqlConnection>();
        private readonly ILogger<GetMovieQueryHandler> _logger = Substitute.For<ILogger<GetMovieQueryHandler>>();

        [Fact]
        public async Task Handle_WithValidCall_ShouldReturnMovieDto()
        {
            // Arrange
            var handler = CreateHandler();
            var query = new GetMovieQuery(Guid.NewGuid());

            _sqlConnection.QuerySingleOrDefaultAsync<MovieDto>(Arg.Any<string>(), Arg.Any<object>())
                .Returns(new MovieDto
                {
                    Title = "Movie 43",
                    Description = "Movie 43 is like if a bunch of A-list actors lost a bet and had to film the weirdest, most random skits imaginable",
                    Genre = "Comedy",
                    Duration = TimeSpan.Parse("01:33:00"),
                    ReleaseDate = DateTime.Parse("2013-01-25"),
                    Directors = "Elizabeth Banks, Peter Farrelly",
                    Cast = "Hugh Jackman, Kate Winslet, Halle Berry, Johnny Knoxville"
                });

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Switch(
                dto => dto.Should().Be(
                    new MovieDto
                    {
                        Title = "Movie 43",
                        Description = "Movie 43 is like if a bunch of A-list actors lost a bet and had to film the weirdest, most random skits imaginable",
                        Genre = "Comedy",
                        Duration = TimeSpan.Parse("01:33:00"),
                        ReleaseDate = DateTime.Parse("2013-01-25"),
                        Directors = "Elizabeth Banks, Peter Farrelly",
                        Cast = "Hugh Jackman, Kate Winslet, Halle Berry, Johnny Knoxville"
                    }),
                notFound => Assert.Fail(),
                error => Assert.Fail()
            );
        }

        [Fact]
        public async Task Handle_WithInvalidCall_ShouldReturnNotFound()
        {
            // Arrange
            var handler = CreateHandler();
            var query = new GetMovieQuery(Guid.NewGuid());

            _sqlConnection.QuerySingleOrDefaultAsync<MovieDto>(Arg.Any<string>(), Arg.Any<object>())
                .Returns(Task.FromResult<MovieDto?>(null));

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Switch(
                dto => Assert.Fail(),
                notFound => notFound.Should().NotBeNull(),
                error => Assert.Fail()
            );
        }

        [Fact]
        public async Task Handle_WithInvalidCall_ShouldReturnApplicationExceptionError()
        {
            // Arrange
            var handler = CreateHandler();
            var query = new GetMovieQuery(Guid.NewGuid());

            _sqlConnection.When(call => call.QuerySingleOrDefaultAsync<MovieDto>(Arg.Any<string>(), Arg.Any<object>())).Do(_ => throw new Exception());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Switch(
                dto => Assert.Fail(),
                notFound => Assert.Fail(),
                error => error.Value.Should().BeOfType<ApplicationException>()
            );
        }

        private GetMovieQueryHandler CreateHandler() => new(_sqlConnection, _logger);
    }
}
