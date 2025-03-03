using Cine.Modules.Tickets.Application.Reservations.GetReservation;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Tickets.Api.Endpoints.Reservations.Get;

internal record Response(
    Guid ReservationId,
    Guid ShowId,
    string StatusType,
    DateTime? ReservedAt,
    DateTime? ConfirmedAt,
    DateTime? PaidAt,
    DateTime? ExpiredAt,
    int SeatsCount);

internal static class DtoConverter
{
    public static Response ToResponse(ReservationDto dto) => new(
        dto.ReservationId,
        dto.ShowId,
        dto.StatusType,
        dto.ReservedAt,
        dto.ConfirmedAt,
        dto.PaidAt,
        dto.ExpiredAt,
        dto.SeatsCount);
}

internal sealed class GetEndpoint(ISender sender) : Ep.NoReq.Res<Response>
{
    public override void Configure()
    {
        Get("reservations/{reservationId:guid}");

        //TODO: Add roles instead
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var reservationId = Route<Guid>("reservationId");

        var oneOf = await sender.Send(new GetReservationQuery(reservationId), ct);
        await oneOf.Match(
            async dto => await SendOkAsync(DtoConverter.ToResponse(dto), ct),
            async notFound => await SendNotFoundAsync(ct),
            error => throw error.Value);
    }
}