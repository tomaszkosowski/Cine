using Cine.Modules.Movies.Application.Movies.CreateMovie;
using Cine.Modules.Movies.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Cine.Modules.Movies.Application.UnitTests.Movies;

public class CreateMovieTests
{
    private readonly IMoviesRepository _moviesRepository = Substitute.For<IMoviesRepository>();
    private readonly IPeopleRepository _peopleRepository = Substitute.For<IPeopleRepository>();
    private readonly ILogger<CreateMovieCommandHandler> _logger = Substitute.For<ILogger<CreateMovieCommandHandler>>();

    [Fact]
    public async Task Handle_WithValidCall_ShouldAddMovie()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateMovieCommand(
            "Movies 43",
            "Movies 43 is like if a bunch of A-list actors lost a bet and had to film the weirdest, most random skits imaginable",
            "Comedy",
            TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(93)),
            new DateOnly(2013, 01, 25),
            [
                ("Peter", "Farrelly"),
                ("Elizabeth", "Banks")
            ],
            [
                ("Hugh", "Jackman"),
                ("Kate", "Winslet"),
                ("Halle", "Berry"),
                ("Johnny", "Knoxville")
            ]);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Switch(
            movieId =>
            {
                movieId.Should().NotBeEmpty();
                _moviesRepository.Received().AddAsync(Arg.Any<Movie>());
            },
            error => Assert.Fail()
        );
    }

    [Fact]
    public async Task Handle_WithInvalidCall_ShouldReturnApplicationExceptionError()
    {
        // Arrange
        _moviesRepository.When(call => call.AddAsync(Arg.Any<Movie>())).Do(_ => throw new Exception());

        var handler = CreateHandler();
        var command = new CreateMovieCommand(
            "Movies 43",
            "Movies 43 is like if a bunch of A-list actors lost a bet and had to film the weirdest, most random skits imaginable",
            "Comedy",
            TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(93)),
            new DateOnly(2013, 01, 25),
            [
                ("Peter", "Farrelly"),
                ("Elizabeth", "Banks")
            ],
            [
                ("Hugh", "Jackman"),
                ("Kate", "Winslet"),
                ("Halle", "Berry"),
                ("Johnny", "Knoxville")
            ]);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Switch(
            movieId => Assert.Fail(),
            error => error.Value.Should().BeOfType<ApplicationException>()
        );
    }

    [Fact]
    public async Task Handle_WithPeopleRepositoryExceptionThrown_ShouldReturnApplicationExceptionError()
    {
        // Arrange
        _peopleRepository.When(call => call.GetAsync(Arg.Any<IReadOnlyList<(string, string)>>())).Do(_ => throw new Exception());

        var handler = CreateHandler();
        var command = new CreateMovieCommand(
            "Movies 43",
            "Movies 43 is like if a bunch of A-list actors lost a bet and had to film the weirdest, most random skits imaginable",
            "Comedy",
            TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(93)),
            new DateOnly(2013, 01, 25),
            [
                ("Peter", "Farrelly"),
                ("Elizabeth", "Banks")
            ],
            [
                ("Hugh", "Jackman"),
                ("Kate", "Winslet"),
                ("Halle", "Berry"),
                ("Johnny", "Knoxville")
            ]);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Switch(
            movieId => Assert.Fail(),
            error => error.Value.Should().BeOfType<ApplicationException>()
        );
    }

    private CreateMovieCommandHandler CreateHandler() => new(_moviesRepository, _peopleRepository, _logger);
}