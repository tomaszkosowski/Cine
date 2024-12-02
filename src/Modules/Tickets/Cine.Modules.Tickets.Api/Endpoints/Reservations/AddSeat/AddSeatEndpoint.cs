using FastEndpoints;

namespace Cine.Modules.Tickets.Api.Endpoints.Reservations.AddSeat;

internal record Request(Guid SeatId);

internal sealed class AddSeatEndpoint : Ep.Req<Request>.NoRes
{
    public override void Configure()
    {
        Post("/reservation/{reservationId:guid}/addSeat");

        //TODO: Add roles instead
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var reservationId = Route<Guid>("reservationId");
        
        await SendOkAsync(ct);
    }
}