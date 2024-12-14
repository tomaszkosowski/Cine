using Cine.Modules.Shows.Application.Shows.CreateShow;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Shows.Api.Endpoints.Show.Add;

public record Request(Guid HallId, Guid MovieId, DateTime StartAt);

public record Response(Guid ShowId);

internal sealed class AddEndpoint(ISender sender) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("show/add");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var oneOf = await sender.Send(new CreateShowCommand(req.HallId, req.MovieId, req.StartAt), ct);

        await oneOf.Match(
            async showId => await SendOkAsync(new Response(showId), ct),
            error => throw error.Value);
    }
}