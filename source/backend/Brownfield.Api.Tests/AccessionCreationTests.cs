using System.Net;
using System.Net.Http.Json;
using Brownfield.Api.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Brownfield.Api.Tests;

public class AccessionCreationTests
{
    [Fact]
    public async Task PostAccession_ReturnsCreatedCanonicalUuidAndPersistsRequiredDescription()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        using var response = await client.PostAsJsonAsync(
            "/api/accessions",
            new { shortDescription = "  Meadow grass from bag 12  " },
            cancellationToken
        );

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateAccessionResponse>(
            cancellationToken
        );
        result.ShouldNotBeNull();
        Guid.TryParseExact(result!.Id, "D", out var id).ShouldBeTrue();
        id.ToString("D").ShouldBe(result.Id);

        await using var connection = new SqliteConnection(factory.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT ShortDescription, StaffNotes FROM Accessions WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", result.Id);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        (await reader.ReadAsync(cancellationToken)).ShouldBeTrue();
        reader.GetString(0).ShouldBe("Meadow grass from bag 12");
        reader.IsDBNull(1).ShouldBeTrue();
    }

    [Fact]
    public async Task PostAccession_PersistsOptionalStaffNotes()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        using var response = await client.PostAsJsonAsync(
            "/api/accessions",
            new
            {
                shortDescription = "Meadow grass",
                staffNotes = "Labels are damaged."
            },
            cancellationToken
        );

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateAccessionResponse>(
            cancellationToken
        );
        result.ShouldNotBeNull();

        await using var connection = new SqliteConnection(factory.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT StaffNotes FROM Accessions WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", result!.Id);
        (await command.ExecuteScalarAsync(cancellationToken)).ShouldBe("Labels are damaged.");
    }

    [Fact]
    public async Task PostAccession_WhenDescriptionIsMissingOrInvalid_ReturnsActionableErrors()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        foreach (var description in new string?[] { null, "", "   ", new string('x', 201) })
        {
            using var response = await client.PostAsJsonAsync(
                "/api/accessions",
                new { shortDescription = description },
                cancellationToken
            );

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
                cancellationToken
            );
            problem.ShouldNotBeNull();
            problem!.Errors.ContainsKey("shortDescription").ShouldBeTrue();
        }
    }

    [Fact]
    public async Task PostAccession_WhenStaffNotesExceedLimit_ReturnsActionableError()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        using var response = await client.PostAsJsonAsync(
            "/api/accessions",
            new { shortDescription = "Meadow grass", staffNotes = new string('x', 2001) },
            cancellationToken
        );

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
            cancellationToken
        );
        problem.ShouldNotBeNull();
        problem!.Errors.ContainsKey("staffNotes").ShouldBeTrue();
    }

    [Fact]
    public async Task PostAccession_WhenIdAlreadyExists_ReturnsConflict()
    {
        const string id = "550e8400-e29b-41d4-a716-446655440000";
        using var factory = new TestApiFactory(new FixedIdGenerator(id));
        using var client = factory.CreateClient();
        var cancellationToken = TestContext.Current.CancellationToken;

        using var created = await client.PostAsJsonAsync(
            "/api/accessions",
            new { shortDescription = "Meadow grass" },
            cancellationToken
        );
        created.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = await created.Content.ReadFromJsonAsync<CreateAccessionResponse>(
            cancellationToken
        );
        result!.Id.ShouldBe(id);

        using var duplicate = await client.PostAsJsonAsync(
            "/api/accessions",
            new { shortDescription = "Meadow grass" },
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
                    client.PostAsJsonAsync(
                        "/api/accessions",
                        new { shortDescription = "Meadow grass" },
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
