using Brownfield.Api.Adapters.Outbound;
using Brownfield.Api.Domain;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is not configured."
    );

builder.Services.AddSingleton<IAccessionIdGenerator, UuidAccessionIdGenerator>();
builder.Services.AddSingleton(new SqliteAccessionStore(connectionString));

var app = builder.Build();

var accessionStore = app.Services.GetRequiredService<SqliteAccessionStore>();
await accessionStore.InitializeAsync(app.Lifetime.ApplicationStopping);

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost(
    "/api/accessions",
    async (
        IAccessionIdGenerator idGenerator,
        SqliteAccessionStore store,
        CancellationToken cancellationToken
    ) =>
    {
        var id = idGenerator.Generate();
        if (!await store.TryCreateAsync(id, cancellationToken))
        {
            return Results.Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "An accession with this identifier already exists."
            );
        }

        return Results.Json(new { id }, statusCode: StatusCodes.Status201Created);
    }
);

app.Run();

public partial class Program { }
