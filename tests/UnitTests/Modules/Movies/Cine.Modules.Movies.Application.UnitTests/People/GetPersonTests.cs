using Cine.Modules.Movies.Application.People.GetPerson;
using Cine.Shared.Application.Database;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Cine.Modules.Movies.Application.UnitTests.People
{
    public class GetPersonTests
    {
        private readonly ISqlConnection _sqlConnection = Substitute.For<ISqlConnection>();
        private readonly ILogger<GetPersonQueryHandler> _logger = Substitute.For<ILogger<GetPersonQueryHandler>>();

        [Fact]
        public async Task Handle_WithValidCall_ShouldReturnPersonDto()
        {
            // Arrange
            var handler = CreateHandler();
            var query = new GetPersonQuery(Guid.NewGuid());

            _sqlConnection.QuerySingleOrDefaultAsync<PersonDto>(Arg.Any<string>(), Arg.Any<object>())
                .Returns(new PersonDto
                {
                    FirstName = "John",
                    LastName = "Doe"
                });

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Switch(
                dto => dto.Should().Be(
                    new PersonDto
                    {
                        FirstName = "John",
                        LastName = "Doe"
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
            var query = new GetPersonQuery(Guid.NewGuid());

            _sqlConnection.QuerySingleOrDefaultAsync<PersonDto>(Arg.Any<string>(), Arg.Any<object>())
                .Returns(Task.FromResult<PersonDto?>(null));

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
            var query = new GetPersonQuery(Guid.NewGuid());

            _sqlConnection.When(call => call.QuerySingleOrDefaultAsync<PersonDto>(Arg.Any<string>(), Arg.Any<object>())).Do(_ => throw new Exception());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Switch(
                dto => Assert.Fail(),
                notFound => Assert.Fail(),
                error => error.Value.Should().BeOfType<ApplicationException>()
            );
        }

        private GetPersonQueryHandler CreateHandler() => new(_sqlConnection, _logger);
    }
}
