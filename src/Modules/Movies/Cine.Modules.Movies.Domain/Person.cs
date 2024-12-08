using Cine.Modules.Movies.Domain.Events;
using Cine.Shared.Domain;
using Cine.Shared.Domain.Rules;

namespace Cine.Modules.Movies.Domain
{
    public record PersonId : TypedId<PersonId>;

    public sealed class Person : Entity, IAggregateRoot
    {
        #region Properties

        public PersonId PersonId { get; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        #endregion

        #region Constructors

        private Person()
        {
            // Blank for ORM.
        }

        public Person(string firstName, string lastName)
        {
            CheckRule(new EnsureNotEmptyRule(firstName, nameof(firstName)));
            CheckRule(new EnsureNotEmptyRule(lastName, nameof(lastName)));

            PersonId = PersonId.Create();

            FirstName = firstName;
            LastName = lastName;

            AddDomainEvent(new PersonCreatedDomainEvent(PersonId));
        }

        #endregion

        #region Public methods

        public void SetFirstName(string firstName)
        {
            CheckRule(new EnsureNotEmptyRule(firstName, nameof(firstName)));

            FirstName = firstName;

            AddDomainEvent(new PersonUpdatedDomainEvent(PersonId, FirstName, LastName));
        }

        public void SetLastName(string lastName)
        {
            CheckRule(new EnsureNotEmptyRule(lastName, nameof(lastName)));

            LastName = lastName;

            AddDomainEvent(new PersonUpdatedDomainEvent(PersonId, FirstName, LastName));
        }

        public static Person Create(string firstName, string lastName) => new(firstName, lastName);

        #endregion
    }
}
