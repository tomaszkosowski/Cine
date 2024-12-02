using Cine.Modules.Shows.IntegrationEvents.Shows;
using Cine.Modules.Tickets.Application.Reservations.CreateReservation;
using Cine.Shared.Infrastructure.Events;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Tickets.Api.Endpoints.Reservations.Create;

internal record Request(Guid ShowId);

internal record Response(Guid ReservationId);

internal sealed class CreateEndpoint(ISender sender) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("reservation/create");

        //TODO: Add roles instead
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var oneOf = await sender.Send(new CreateReservationCommand(req.ShowId), ct);
        
        await oneOf.Match(
            async reservationId => await SendOkAsync(new Response(reservationId), ct),
            error => throw error.Value
        );
    }
}