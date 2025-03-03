using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Sales.IntegrationEvents.Payments;

public sealed record PaymentConfirmedIntegrationEvent(Guid ReservationId) : IntegrationEvent;