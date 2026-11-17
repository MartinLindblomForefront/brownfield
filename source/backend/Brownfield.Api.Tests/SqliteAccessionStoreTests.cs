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
            (await store.TryCreateAsync("same-id", cancellationToken)).ShouldBeTrue();
            (await store.TryCreateAsync("same-id", cancellationToken)).ShouldBeFalse();
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
    ) => store.TryCreateAsync(id, cancellationToken);
}
