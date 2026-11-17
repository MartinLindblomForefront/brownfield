using System.Net;
using System.Net.Http.Json;
using Brownfield.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Brownfield.Api.Tests;

public class AccessionCreationTests
{
    [Fact]
    public async Task PostAccession_ReturnsCreatedCanonicalUuid()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        using var response = await client.PostAsync(
            "/api/accessions",
            content: null,
            cancellationToken
        );

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateAccessionResponse>(
            cancellationToken
        );
        result.ShouldNotBeNull();
        Guid.TryParseExact(result!.Id, "D", out var id).ShouldBeTrue();
        id.ToString("D").ShouldBe(result.Id);
    }

    [Fact]
    public async Task PostAccession_WhenIdAlreadyExists_ReturnsConflict()
    {
        const string id = "550e8400-e29b-41d4-a716-446655440000";
        using var factory = new TestApiFactory(new FixedIdGenerator(id));
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        using var created = await client.PostAsync(
            "/api/accessions",
            content: null,
            cancellationToken
        );
        created.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = await created.Content.ReadFromJsonAsync<CreateAccessionResponse>(
            cancellationToken
        );
        result!.Id.ShouldBe(id);

        using var duplicate = await client.PostAsync(
            "/api/accessions",
            content: null,
            cancellationToken
        );
        duplicate.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        var problem = await duplicate.Content.ReadFromJsonAsync<ProblemDetails>(
            cancellationToken
        );
        problem.ShouldNotBeNull();
        problem!.Title.ShouldBe("An accession with this identifier already exists.");
    }

    [Fact]
    public async Task PostAccession_WhenConcurrentRequestsGenerateSameId_RejectsDuplicates()
    {
        using var factory = new TestApiFactory(
            new FixedIdGenerator("550e8400-e29b-41d4-a716-446655440000")
        );
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        var responses = await Task.WhenAll(
            Enumerable
                .Range(0, 8)
                .Select(_ =>
                    client.PostAsync(
                        "/api/accessions",
                        content: null,
                        cancellationToken
                    )
                )
        );

        try
        {
            var responseDetails = await Task.WhenAll(
                responses.Select(async response =>
                    $"{(int)response.StatusCode}: {await response.Content.ReadAsStringAsync(cancellationToken)}"
                )
            );
            responses
                .Count(response => response.StatusCode == HttpStatusCode.Created)
                .ShouldBe(1, string.Join("; ", responseDetails));
            responses
                .Count(response => response.StatusCode == HttpStatusCode.Conflict)
                .ShouldBe(7);
        }
        finally
        {
            foreach (var response in responses)
            {
                response.Dispose();
            }
        }
    }

    private sealed record CreateAccessionResponse(string Id);

    private sealed class FixedIdGenerator(string id) : IAccessionIdGenerator
    {
        public string Generate() => id;
    }
}
