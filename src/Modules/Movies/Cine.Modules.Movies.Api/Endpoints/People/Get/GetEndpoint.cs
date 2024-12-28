using Cine.Modules.Movies.Application.People.GetPerson;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Movies.Api.Endpoints.People.Get;

internal record Request(Guid PersonId);

internal record Response(PersonDto Dto);

internal sealed class GetEndpoint(ISender sender) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/person/get");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var oneOf = await sender.Send(new GetPersonQuery(req.PersonId), ct);

        await oneOf.Match(
            async dto => await SendOkAsync(new(dto), ct),
            async notFound => await SendNotFoundAsync(ct),
            error => throw error.Value);
    }
}