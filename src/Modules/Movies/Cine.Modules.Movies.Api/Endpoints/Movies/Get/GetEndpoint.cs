using Cine.Modules.Movies.Application.Movies.GetMovie;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Movies.Api.Endpoints.Movies.Get
{
    internal record Request(Guid MovieId);

    internal record Response(MovieDto Dto);

    internal sealed class GetEndpoint(ISender _sender) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Get("movie/get");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var oneOf = await _sender.Send(new GetMovieQuery(req.MovieId), ct);

            await oneOf.Match(
                async dto => await SendOkAsync(new(dto), ct),
                async notFound => await SendNotFoundAsync(ct),
                error => throw error.Value);
        }
    }
}
