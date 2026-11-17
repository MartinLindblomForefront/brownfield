using System.Net;
using System.Net.Http.Json;
using Shouldly;

namespace Brownfield.Api.Tests;

public class HealthEndpointTests(TestApiFactory factory) : IClassFixture<TestApiFactory>
{
    [Fact]
    public async Task GetHealth_ReturnsOkAndStatus()
    {
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;
        using var response = await client.GetAsync("/health", cancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<HealthResponse>(
            cancellationToken
        );
        result.ShouldNotBeNull();
        result!.Status.ShouldBe("ok");
    }

    private sealed record HealthResponse(string Status);
}
