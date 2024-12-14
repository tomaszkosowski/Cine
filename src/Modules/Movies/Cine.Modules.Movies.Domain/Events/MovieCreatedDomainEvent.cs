using Cine.Shared.Domain.Events;

namespace Cine.Modules.Movies.Domain.Events
{
    public record MovieCreatedDomainEvent(MovieId MovieId) : DomainEvent;
}
