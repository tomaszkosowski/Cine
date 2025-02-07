using Cine.Modules.Tickets.Application.Reservations.ConfirmReservation;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Tickets.Api.Endpoints.Reservations.Confirm;

internal sealed class ConfirmEndpoint(ISender sender) : Ep.NoReq.NoRes
{
    public override void Configure()
    {
        Post("reservation/{reservationId:guid}/confirm");

        //TODO: Add roles instead
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var reservationId = Route<Guid>("reservationId");

        var oneOf = await sender.Send(new ConfirmReservationCommand(reservationId), ct);
        await oneOf.Match(
            async success => await SendOkAsync(ct),
            async notFound => await SendNotFoundAsync(ct),
            error => throw error.Value);
    }
}