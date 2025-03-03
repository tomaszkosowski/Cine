using FastEndpoints;

namespace Cine.Modules.Sales.Api.Endpoints;

public class Hello : Ep.NoReq.NoRes
{
    public override void Configure()
    {
        Get("/hello");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(ct);
    }
}