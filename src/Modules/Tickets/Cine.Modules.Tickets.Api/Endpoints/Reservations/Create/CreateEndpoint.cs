using FastEndpoints;

namespace Cine.Modules.Tickets.Api.Endpoints.Reservations.Create;

internal record Request;

internal record Response;

internal sealed class CreateEndpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("reservation/create");
        
        //TODO: Add roles instead
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await SendOkAsync(ct);
    }
}