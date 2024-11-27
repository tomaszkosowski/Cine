using Cine.Modules.Movies.Domain.Events;
using Cine.Modules.Movies.Domain.UnitTests.Factories;
using Cine.Shared.Domain.Rules;
using Cine.Shared.Domain.UnitTests;
using FluentAssertions;

namespace Cine.Modules.Movies.Domain.UnitTests
{
    public class PersonTests
    {
        [Fact]
        public void Create_WithValidData_ShouldPublishPersonCreatedDomainEvent()
        {
            // Arrange
            var createPerson = () => PersonObjectFactory.CreateValidObject("John", "Doe");

            // Act
            var person = createPerson();

            // Assert
            var domainEvent = person.GetDomainEvent<PersonCreatedDomainEvent>();

            domainEvent.Should().NotBeNull();
            domainEvent?.PersonId.Should().Be(person.PersonId);
        }

        [Fact]
        public void Create_WithEmptyFirstName_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var createPerson = () => PersonObjectFactory.CreateValidObject("", "Doe");

            // Act & Assert
            createPerson.AssertBrokenRule<EnsureNotEmptyRule>();
        }

        [Fact]
        public void Create_WithEmptyLastName_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var createPerson = () => PersonObjectFactory.CreateValidObject("John", "");

            // Act & Assert
            createPerson.AssertBrokenRule<EnsureNotEmptyRule>();
        }
    }
}
