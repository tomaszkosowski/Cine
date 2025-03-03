using Cine.Modules.Sales.IntegrationEvents.Payments;
using Cine.Modules.Tickets.Application.Reservations.CompleteReservation;
using Cine.Shared.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Tickets.Application.Payments;

internal sealed class PaymentConfirmedIntegrationEventHandler(IServiceProvider serviceProvider)
    : IIntegrationEventHandler<PaymentConfirmedIntegrationEvent>
{
    public async Task HandleAsync(PaymentConfirmedIntegrationEvent @event)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await sender.Send(new CompleteReservationCommand(@event.ReservationId));
    }
}