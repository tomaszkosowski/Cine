using Cine.Shared.Domain.Events;

namespace Cine.Modules.Movies.Domain.Events
{
    public record MovieUpdatedDomainEvent(MovieId MovieId) : IDomainEvent;
}
