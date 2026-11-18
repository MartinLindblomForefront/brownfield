using Brownfield.Api.Adapters.Outbound;
using Microsoft.Data.Sqlite;
using Shouldly;

namespace Brownfield.Api.Tests;

public class SqliteAccessionStoreTests
{
    [Fact]
    public async Task TryCreateAsync_RejectsDuplicateIdentifiers()
    {
        var store = CreateStore(out var databasePath);
        var cancellationToken = TestContext.Current.CancellationToken;
        try
        {
            await store.InitializeAsync(cancellationToken);
            (
                await store.TryCreateAsync(
                    "same-id",
                    "Meadow grass",
                    staffNotes: null,
                    cancellationToken
                )
            ).ShouldBeTrue();
            (
                await store.TryCreateAsync(
                    "same-id",
                    "Meadow grass",
                    staffNotes: null,
                    cancellationToken
                )
            ).ShouldBeFalse();
        }
        finally
        {
            File.Delete(databasePath);
        }
    }

    [Fact]
    public async Task TryCreateAsync_WhenConcurrentRequestsUseSameId_PersistsOnlyOne()
    {
        var store = CreateStore(out var databasePath);
        var cancellationToken = TestContext.Current.CancellationToken;
        try
        {
            await store.InitializeAsync(cancellationToken);
            var results = await Task.WhenAll(
                Enumerable
                    .Range(0, 8)
                    .Select(_ => TryCreateAsync(store, "same-id", cancellationToken))
            );

            results.Count(created => created).ShouldBe(1);

            await using var connection = new SqliteConnection(
                $"Data Source={databasePath};Pooling=False"
            );
            await connection.OpenAsync(cancellationToken);
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Accessions WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", "same-id");

            Convert
                .ToInt32(await command.ExecuteScalarAsync(cancellationToken))
                .ShouldBe(1);
        }
        finally
        {
            File.Delete(databasePath);
        }
    }

    [Fact]
    public async Task InitializeAsync_AddsIdentificationColumnsToExistingDatabase()
    {
        var store = CreateStore(out var databasePath);
        var cancellationToken = TestContext.Current.CancellationToken;
        try
        {
            await using (var connection = new SqliteConnection(
                $"Data Source={databasePath};Pooling=False"
            ))
            {
                await connection.OpenAsync(cancellationToken);
                await using var command = connection.CreateCommand();
                command.CommandText =
                    """
                    CREATE TABLE Accessions (Id TEXT NOT NULL PRIMARY KEY);
                    INSERT INTO Accessions (Id) VALUES ('existing-id');
                    """;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            await store.InitializeAsync(cancellationToken);
            (
                await store.TryCreateAsync(
                    "new-id",
                    "Meadow grass",
                    "Collected from bag 12.",
                    cancellationToken
                )
            ).ShouldBeTrue();

            await using var verification = new SqliteConnection(
                $"Data Source={databasePath};Pooling=False"
            );
            await verification.OpenAsync(cancellationToken);
            await using var query = verification.CreateCommand();
            query.CommandText =
                """
                SELECT ShortDescription, StaffNotes
                FROM Accessions
                WHERE Id = $id;
                """;
            query.Parameters.AddWithValue("$id", "new-id");
            await using var reader = await query.ExecuteReaderAsync(cancellationToken);
            (await reader.ReadAsync(cancellationToken)).ShouldBeTrue();
            reader.GetString(0).ShouldBe("Meadow grass");
            reader.GetString(1).ShouldBe("Collected from bag 12.");

            await reader.CloseAsync();
            query.CommandText = "SELECT COUNT(*) FROM Accessions WHERE Id = 'existing-id';";
            Convert
                .ToInt32(await query.ExecuteScalarAsync(cancellationToken))
                .ShouldBe(1);
        }
        finally
        {
            File.Delete(databasePath);
        }
    }

    private static SqliteAccessionStore CreateStore(out string databasePath)
    {
        databasePath = Path.Combine(
            Path.GetTempPath(),
            $"brownfield-{Guid.NewGuid():N}.db"
        );
        return new SqliteAccessionStore($"Data Source={databasePath};Pooling=False");
    }

    private static Task<bool> TryCreateAsync(
        SqliteAccessionStore store,
        string id,
        CancellationToken cancellationToken
    ) => store.TryCreateAsync(id, "Meadow grass", staffNotes: null, cancellationToken);
}
