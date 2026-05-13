using Cine.Modules.Movies.Application.Movies.GetMovie;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Movies.Api.Endpoints.Movies.Get;

internal record Request(Guid MovieId);

internal record Response(MovieDto Dto);

internal sealed class GetEndpoint(ISender sender) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("movie/get");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var oneOf = await sender.Send(new GetMovieQuery(req.MovieId), ct);

        await oneOf.Match(
            async dto => await Send.OkAsync(new Response(dto), ct),
            async notFound => await Send.NotFoundAsync(ct),
            error => throw error.Value);
    }
}