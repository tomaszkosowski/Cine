using Cine.Modules.Movies.Application.Movies.CreateMovie;
using FastEndpoints.Testing;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Movies.IntegrationTests.Movies;

[CollectionDefinition(nameof(CreateMovieTests))]
public class CreateMovieTestCollection : TestCollection<App>;

[Collection(nameof(CreateMovieTests))]
public class CreateMovieTests(App app) : IntegrationTestBase(app)
{
    [Fact]
    public async Task CreateMovie_WhenValidCommand_ShouldReturnMovieId()
    {
        var command = new CreateMovieCommand(
            "Movie 43",
            "Movie 43 is like if a bunch of A-list actors lost a bet and had to film the weirdest, most random skits imaginable",
            "Comedy",
            TimeOnly.Parse("01:33:00"),
            DateOnly.Parse("2013-01-25"),
            [
                new("Elizabeth", "Banks"),
                new("Peter", "Farrelly")
            ],
            [
                new("Hugh", "Jackman"),
                new("Kate", "Winslet"),
                new("Halle", "Berry"),
                new("Johnny", "Knoxville")
            ]);

        var result = await Sender.Send(command);

        result.IsT0.Should().BeTrue();
        result.AsT0.Should().NotBeEmpty();
    }
}