using Cine.Modules.Movies.Api.Endpoints.People.Get;
using Cine.Modules.Movies.Application.People.CreatePerson;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Movies.Api.Endpoints.People.Add
{
    internal record Request(string FirstName, string LastName);

    internal record Response(Guid PersonId);

    internal sealed class AddEnpoint(ISender _sender) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/person/add");

            // TODO: Add roles instead
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var oneOf = await _sender.Send(new CreatePersonCommand(req.FirstName, req.LastName), ct);

            await oneOf.Match(
                async personId => await SendCreatedAtAsync<GetEndpoint>(personId, new(personId), cancellation: ct),
                error => throw error.Value);
        }
    }
}
