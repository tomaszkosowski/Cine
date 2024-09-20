using Cine.Modules.Movies.Api.Endpoints.Movies.Get;
using Cine.Modules.Movies.Application.Movies.CreateMovie;
using FastEndpoints;
using MediatR;

namespace Cine.Modules.Movies.Api.Endpoints.Movies.Add
{
    internal record Request(string Title, string Description, string Genre, TimeOnly Duration, DateOnly ReleaseDate, List<Person> Directors, List<Person> Cast);

    internal record Response(Guid MovieId);

    internal record Person(string FirstName, string LastName)
    {
        internal (string FirstName, string LastName) Deconstruct() => (FirstName, LastName);
    }

    internal sealed class AddEndpoint(ISender _sender) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("movie/add");

            //TODO: Add roles instead
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var directors = req.Directors.Select(person => person.Deconstruct()).ToList();
            var cast = req.Cast.Select(person => person.Deconstruct()).ToList();

            var oneOf = await _sender.Send(new CreateMovieCommand(req.Title, req.Description, req.Genre, req.Duration, req.ReleaseDate, directors.AsReadOnly(), cast.AsReadOnly()), ct);

            await oneOf.Match(
                async movieId => await SendCreatedAtAsync<GetEndpoint>(movieId, new(movieId), cancellation: ct),
                error => throw error.Value);
        }
    }
}
