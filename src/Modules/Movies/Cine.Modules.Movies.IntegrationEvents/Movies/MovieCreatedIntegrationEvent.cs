using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Movies.IntegrationEvents.Movies;

public record MovieCreatedIntegrationEvent(Guid MovieId, TimeSpan Duration) : IntegrationEvent;