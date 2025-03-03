using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Tickets.IntegrationEvents.Reservations;

public record ReservationConfirmedIntegrationEvent(Guid ReservationId) : IntegrationEvent;