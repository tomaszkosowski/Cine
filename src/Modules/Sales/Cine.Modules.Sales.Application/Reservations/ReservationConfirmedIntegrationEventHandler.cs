using Cine.Modules.Sales.Application.Payments.CreatePayment;
using Cine.Modules.Tickets.IntegrationEvents.Reservations;
using Cine.Shared.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OneOf.Types;

namespace Cine.Modules.Sales.Application.Reservations;

internal sealed class ReservationConfirmedIntegrationEventHandler(IServiceProvider serviceProvider)
    : IIntegrationEventHandler<ReservationConfirmedIntegrationEvent>
{
    public async Task HandleAsync(ReservationConfirmedIntegrationEvent @event)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await sender.Send(new CreatePaymentCommand(@event.ReservationId));
    }
}