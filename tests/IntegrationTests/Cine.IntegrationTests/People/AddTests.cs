using Cine.Modules.Movies.Api.Endpoints.People;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;

namespace Cine.IntegrationTests.People
{
    public class AddTests(MoviesApiApp _app) : TestBase<MoviesApiApp>
    {
        [Fact, Priority(1)]
        public async Task AddPerson()
        {
            var (http, response) = await _app.Client.POSTAsync<AddEnpoint, Request, Response>(new("John", "Doe"));

            http.IsSuccessStatusCode.Should().BeTrue();
            response.PersonId.Should().NotBeEmpty();
        }
    }
}
