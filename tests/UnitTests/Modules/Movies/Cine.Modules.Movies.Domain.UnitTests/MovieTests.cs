using Cine.Modules.Movies.Domain.Events;
using Cine.Modules.Movies.Domain.UnitTests.Factories;
using Cine.Shared.Domain.Rules;
using Cine.Shared.Domain.UnitTests;
using FluentAssertions;

namespace Cine.Modules.Movies.Domain.UnitTests;

public class MovieTests
{
    [Fact]
    public void Create_WithValidData_ShouldPublishMovieCreatedDomainEvent()
    {
        // Arrange
        var createMovie = () => MovieObjectFactory.CreateValidObject();

        // Act
        var movie = createMovie();

        // Assert
        var domainEvent = movie.GetDomainEvent<MovieCreatedDomainEvent>();

        domainEvent.Should().NotBeNull();
        domainEvent?.MovieId.Should().Be(movie.MovieId);

    }

    [Fact]
    public void Create_WithInvalidData_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var createMovie = () => MovieObjectFactory.CreateInvalidObject();

        // Act & Assert
        createMovie.AssertBrokenRule<EnsureNotEmptyRule>();
    }

    [Fact]
    public void AddDirectors_WithValidData_ShouldPublishMovieUpdatedDomainEvent()
    {
        // Arrange
        var director1 = Person.Create("John", "Doe");
        var director2 = Person.Create("Clint", "Eastwood");

        var directors = new List<Person>()
        {
            director1,
            director2
        };

        var movie = MovieObjectFactory.CreateValidObject();

        // Act
        movie.AddDirectors(directors);

        // Assert
        var domainEvent = movie.GetDomainEvent<MovieUpdatedDomainEvent>();

        domainEvent.Should().NotBeNull();
        domainEvent?.MovieId?.Should().Be(movie.MovieId);
    }

    [Fact]
    public void AddDirectors_WithEmptyData_ShouldNotPublishMovieUpdatedDomainEvent()
    {
        // Arrange
        var movie = MovieObjectFactory.CreateValidObject();

        // Act
        movie.AddDirectors([]);

        // Assert
        var domainEvent = movie.GetDomainEvent<MovieUpdatedDomainEvent>();

        domainEvent.Should().BeNull();
    }

    [Fact]
    public void AddCast_WithValidData_ShouldPublishMovieUpdatedDomainEvent()
    {
        // Arrange
        var cast1 = Person.Create("John", "Doe");
        var cast2 = Person.Create("Clint", "Eastwood");

        var cast = new List<Person>()
        {
            cast1,
            cast2
        };

        var movie = MovieObjectFactory.CreateValidObject();

        // Act
        movie.AddCast(cast);

        // Assert
        var domainEvent = movie.GetDomainEvent<MovieUpdatedDomainEvent>();

        domainEvent.Should().NotBeNull();
        domainEvent?.MovieId?.Should().Be(movie.MovieId);
    }

    [Fact]
    public void AddCast_WithEmptyData_ShouldNotPublishMovieUpdatedDomainEvent()
    {
        // Arrange
        var movie = MovieObjectFactory.CreateValidObject();

        // Act
        movie.AddCast([]);

        // Assert
        var domainEvent = movie.GetDomainEvent<MovieUpdatedDomainEvent>();

        domainEvent.Should().BeNull();
    }
}