using Cine.Modules.Theater.Application.Halls.CreateHall;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Theater.Api.Endpoints.Halls.Add;

internal record Request(string Name, int SeatRows, int SeatsPerRow, List<PremiumSeatDto> PremiumSeats);

internal record Response(Guid HallId, List<SeatDto> Seats);

internal record PremiumSeatDto(string Row, int Number) : SeatDto(Row, Number, "Premium");

internal sealed class AddEndpoint(ISender sender) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("hall/add");

        // TODO: Add roles instead
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var layout = (req.SeatRows, req.SeatsPerRow);
        var premiumSeats = req.PremiumSeats.Select(seat => (row: seat.Row, seat.Number)).ToList();

        var oneOf = await sender.Send(new CreateHallCommand(req.Name, layout), ct);

        await oneOf.Match(
            async result => await Send.OkAsync(new Response(result.HallId, result.Seats.ToList()), ct),
            error => throw error.Value);
    }
}