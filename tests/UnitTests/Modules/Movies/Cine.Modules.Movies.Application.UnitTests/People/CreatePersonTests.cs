using Cine.Modules.Movies.Application.People.CreatePerson;
using Cine.Modules.Movies.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Cine.Modules.Movies.Application.UnitTests.People;

public class CreatePersonTests
{
    private readonly IPeopleRepository _peopleRepository = Substitute.For<IPeopleRepository>();
    private readonly ILogger<CreatePersonCommandHandler> _logger = Substitute.For<ILogger<CreatePersonCommandHandler>>();

    [Fact]
    public async Task Handle_WithValidCall_ShouldAddPerson()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreatePersonCommand("John", "Doe");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Switch(
            personId =>
            {
                personId.Should().NotBeEmpty();
                _peopleRepository.Received().AddAsync(Arg.Any<Person>());
            },
            error => Assert.Fail()
        );
    }

    [Fact]
    public async Task Handle_WithInvalidCall_ShouldReturnApplicationExceptionError()
    {
        // Arrange
        _peopleRepository.When(call => call.AddAsync(Arg.Any<Person>())).Do(_ => throw new Exception());

        var handler = CreateHandler();
        var command = new CreatePersonCommand("John", "Doe");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Switch(
            personId => Assert.Fail(),
            error => error.Value.Should().BeOfType<ApplicationException>()
        );
    }

    private CreatePersonCommandHandler CreateHandler() => new(_peopleRepository, _logger);
}