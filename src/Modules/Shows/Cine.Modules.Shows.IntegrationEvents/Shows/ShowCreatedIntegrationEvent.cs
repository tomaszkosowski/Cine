using Cine.Shared.Domain;
using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Shows.IntegrationEvents.Shows;

public sealed record ShowCreatedIntegrationEvent(Guid ShowId) : IntegrationEvent;