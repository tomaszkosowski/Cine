using Cine.Shared.Domain.Events;

namespace Cine.Modules.Movies.Domain.Events;

public record PersonUpdatedDomainEvent(PersonId PersonId, string FirstName, string LastName) : IDomainEvent;