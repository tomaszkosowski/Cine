using Cine.Modules.Movies.Domain.Events;
using Cine.Shared.Application.Events;

namespace Cine.Modules.Movies.Application.Movies.MovieCreated;

public record MovieCreatedNotification(Guid id, MovieCreatedDomainEvent domainEvent)
    : DomainEventNotification<MovieCreatedDomainEvent>(id, domainEvent);