using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Theater.IntegrationEvents.Halls;

public record HallCreatedIntegrationEvent(Guid HallId) : IntegrationEvent;