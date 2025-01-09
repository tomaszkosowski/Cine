using Cine.Modules.Theater.Application.Seats;
using Cine.Modules.Theater.Application.Seats.GetSeats;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Theater.Api.Endpoints.Halls.Get;

internal record Response(List<SeatDto> Seats);

internal sealed class GetEndpoint(ISender sender) : Ep.NoReq.Res<Response>
{
    public override void Configure()
    {
        Get("halls/{hallId:guid}/seats");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var hallId = Route<Guid>("hallId");

        var oneOf = await sender.Send(new GetSeatsQuery(hallId), ct);
        await oneOf.Match(
            async seatDtos => await SendOkAsync(new Response(seatDtos.ToList()), ct),
            error => throw error.Value);
    }
}