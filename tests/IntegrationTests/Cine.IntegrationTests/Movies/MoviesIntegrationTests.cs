using Cine.Modules.Movies.Api.Endpoints.Movies.Get;
using Cine.Shared.Application.Tasks;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Snapshooter.Xunit;
using AddMovieEndpoint = Cine.Modules.Movies.Api.Endpoints.Movies.Add.AddEndpoint;
using AddMovieRequest = Cine.Modules.Movies.Api.Endpoints.Movies.Add.Request;
using AddMovieResponse = Cine.Modules.Movies.Api.Endpoints.Movies.Add.Response;
using AddPersonEndpoint = Cine.Modules.Movies.Api.Endpoints.People.Add.AddEnpoint;
using AddPersonRequest = Cine.Modules.Movies.Api.Endpoints.People.Add.Request;
using AddPersonResponse = Cine.Modules.Movies.Api.Endpoints.People.Add.Response;
using GetMovieRequest = Cine.Modules.Movies.Api.Endpoints.Movies.Get.Request;
using GetMovieResponse = Cine.Modules.Movies.Api.Endpoints.Movies.Get.Response;

namespace Cine.IntegrationTests.Movies
{
    public class MoviesIntegrationTests(MoviesApiApp _app) : TestBase<MoviesApiApp>, IAsyncLifetime
    {
        [Fact]
        public async Task Add_WhenValidRequest_ShouldReturnValidResponse()
        {
            async Task<bool> AddDirectorsAsync()
            {
                var results = await Task.WhenAll(
                    CallAddPersonAsync("Elizabeth", "Banks"),
                    CallAddPersonAsync("Peter", "Farrelly"));

                return results.All(result => result.IsSuccessStatusCode);
            }

            async Task<bool> AddCastAsync()
            {
                var results = await Task.WhenAll(
                    CallAddPersonAsync("Hugh", "Jackman"),
                    CallAddPersonAsync("Kate", "Winslet"),
                    CallAddPersonAsync("Halle", "Berry"),
                    CallAddPersonAsync("Johnny", "Knoxville"));

                return results.All(result => result.IsSuccessStatusCode);
            }

            var (directorsOk, castOk) = await (AddDirectorsAsync(), AddCastAsync());

            directorsOk.Should().BeTrue();
            castOk.Should().BeTrue();

            var (addHttp, addResponse) = await CallAddMovieAsync();
            addHttp.IsSuccessStatusCode.Should().BeTrue();

            var (getHttp, getResponse) = await CallGetMovieAsync(addResponse.MovieId);
            getHttp.IsSuccessStatusCode.Should().BeTrue();

            Snapshot.Match(getResponse);
        }

        private async Task<HttpResponseMessage> CallAddPersonAsync(string firstName, string lastName)
        {
            var request = new AddPersonRequest(firstName, lastName);

            var (http, _) = await _app.Client.POSTAsync<AddPersonEndpoint, AddPersonRequest, AddPersonResponse>(request);

            return http;
        }

        private async Task<(HttpResponseMessage, AddMovieResponse)> CallAddMovieAsync()
        {
            var request = new AddMovieRequest(
                "Movie 43",
                "Movie 43 is like if a bunch of A-list actors lost a bet and had to film the weirdest, most random skits imaginable",
                "Comedy",
                TimeOnly.Parse("01:33:00"),
                DateOnly.Parse("2013-01-25"),
                [
                    new("Elizabeth", "Banks"),
                    new("Peter", "Farrelly")],
                [
                    new("Hugh", "Jackman"),
                    new("Kate", "Winslet"),
                    new("Halle", "Berry"),
                    new("Johnny", "Knoxville")
                ]);

            var (http, response) = await _app.Client.POSTAsync<AddMovieEndpoint, AddMovieRequest, AddMovieResponse>(request);

            return (http, response);
        }

        private async Task<(HttpResponseMessage, GetMovieResponse)> CallGetMovieAsync(Guid personId)
        {
            var request = new GetMovieRequest(personId);

            var (http, response) = await _app.Client.GETAsync<GetEndpoint, GetMovieRequest, GetMovieResponse>(request);

            return (http, response);
        }
    }
}
