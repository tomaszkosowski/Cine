using Cine.Shared.Domain.Events;

namespace Cine.Modules.Shows.Domain.Events;

public record ShowCreatedDomainEvent(ShowId ShowId) : IDomainEvent;