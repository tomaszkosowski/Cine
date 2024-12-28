using Cine.Modules.Movies.Api.Endpoints.People.Add;
using Cine.Modules.Movies.Api.Endpoints.People.Get;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Snapshooter.Xunit;
using AddRequest = Cine.Modules.Movies.Api.Endpoints.People.Add.Request;
using AddResponse = Cine.Modules.Movies.Api.Endpoints.People.Add.Response;
using GetRequest = Cine.Modules.Movies.Api.Endpoints.People.Get.Request;
using GetResponse = Cine.Modules.Movies.Api.Endpoints.People.Get.Response;

namespace Cine.IntegrationTests.People;

public class PeopleIntegrationTests(PeopleApiApp app) : TestBase<PeopleApiApp>, IAsyncLifetime
{
    [Fact]
    public async Task Add_WhenValidRequest_ShouldReturnValidResponse()
    {
        var (addHttp, addResponse) = await CallAddAsync("John", "Doe");
        addHttp.IsSuccessStatusCode.Should().BeTrue();

        var (getHttp, getResponse) = await CallGetAsync(addResponse.PersonId);
        getHttp.IsSuccessStatusCode.Should().BeTrue();

        Snapshot.Match(getResponse);
    }

    private async Task<(HttpResponseMessage, AddResponse)> CallAddAsync(string firstName, string lastName)
    {
        var request = new AddRequest(firstName, lastName);

        var (http, response) = await app.Client.POSTAsync<AddEnpoint, AddRequest, AddResponse>(request);

        return (http, response);
    }

    private async Task<(HttpResponseMessage, GetResponse)> CallGetAsync(Guid personId)
    {
        var request = new GetRequest(personId);

        var (http, response) = await app.Client.GETAsync<GetEndpoint, GetRequest, GetResponse>(request);

        return (http, response);
    }
}