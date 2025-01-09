using Cine.Modules.Tickets.Application.Reservations.AddSeatToReservation;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Tickets.Api.Endpoints.Reservations.AddSeat;

internal sealed class AddSeatEndpoint(ISender sender) : Ep.NoReq.NoRes
{
    public override void Configure()
    {
        Post("/reservation/{reservationId:guid}/addSeat/{seatId:guid}");

        //TODO: Add roles instead
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var reservationId = Route<Guid>("reservationId");
        var seatId = Route<Guid>("seatId");

        var oneOf = await sender.Send(new AddSeatToReservationCommand(reservationId, seatId), ct);

        await oneOf.Match(
            async success => await SendOkAsync(ct),
            error => throw error.Value
        );
    }
}