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
            """
            CREATE TABLE IF NOT EXISTS Accessions (
                Id TEXT NOT NULL PRIMARY KEY,
                ShortDescription TEXT NOT NULL,
                StaffNotes TEXT NULL
            );
            """;
        await command.ExecuteNonQueryAsync(cancellationToken);

        var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        await using (var columnCommand = connection.CreateCommand())
        {
            columnCommand.CommandText = "PRAGMA table_info(Accessions);";
            await using var reader = await columnCommand.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                columns.Add(reader.GetString(1));
            }
        }

        if (!columns.Contains("ShortDescription"))
        {
            await using var migration = connection.CreateCommand();
            migration.CommandText =
                "ALTER TABLE Accessions ADD COLUMN ShortDescription TEXT NOT NULL DEFAULT '';";
            await migration.ExecuteNonQueryAsync(cancellationToken);
        }

        if (!columns.Contains("StaffNotes"))
        {
            await using var migration = connection.CreateCommand();
            migration.CommandText = "ALTER TABLE Accessions ADD COLUMN StaffNotes TEXT NULL;";
            await migration.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task<bool> TryCreateAsync(
        string id,
        string shortDescription,
        string? staffNotes,
        CancellationToken cancellationToken = default
    )
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Accessions (Id, ShortDescription, StaffNotes)
            VALUES ($id, $shortDescription, $staffNotes);
            """;
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$shortDescription", shortDescription);
        command.Parameters.AddWithValue("$staffNotes", (object?)staffNotes ?? DBNull.Value);
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
