using Microsoft.Data.Sqlite;

namespace Brownfield.Api.Adapters.Outbound;

public sealed class SqliteAccessionStore(string connectionString)
{
    private const int PrimaryKeyConstraintError = 1555;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText =
            "CREATE TABLE IF NOT EXISTS Accessions (Id TEXT NOT NULL PRIMARY KEY);";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<bool> TryCreateAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Accessions (Id) VALUES ($id);";
        command.Parameters.AddWithValue("$id", id);
        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
            return true;
        }
        catch (SqliteException exception)
            when (exception.SqliteExtendedErrorCode == PrimaryKeyConstraintError)
        {
            return false;
        }
    }
}
