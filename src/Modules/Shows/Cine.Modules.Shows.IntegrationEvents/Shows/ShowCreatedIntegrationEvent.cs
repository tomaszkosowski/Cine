using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Shows.IntegrationEvents.Shows;

public sealed record ShowCreatedIntegrationEvent(Guid ShowId, Guid HallId, DateTime StartAt) : IntegrationEvent;