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
        CreateAccessionRequest? request,
        IAccessionIdGenerator idGenerator,
        SqliteAccessionStore store,
        CancellationToken cancellationToken
    ) =>
    {
        var errors = new Dictionary<string, string[]>();
        if (request is null)
        {
            errors["request"] = ["A request body is required."];
            return Results.ValidationProblem(errors);
        }

        var shortDescription = request.ShortDescription?.Trim();
        if (string.IsNullOrEmpty(shortDescription))
        {
            errors["shortDescription"] = ["Enter a short description."];
        }
        else if (shortDescription.Length > 200)
        {
            errors["shortDescription"] = ["Use 200 characters or fewer."];
        }

        if (request.StaffNotes?.Length > 2000)
        {
            errors["staffNotes"] = ["Use 2,000 characters or fewer."];
        }

        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        var staffNotes = string.IsNullOrWhiteSpace(request.StaffNotes)
            ? null
            : request.StaffNotes;
        var id = idGenerator.Generate();
        if (
            !await store.TryCreateAsync(
                id,
                shortDescription!,
                staffNotes,
                cancellationToken
            )
        )
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

public sealed record CreateAccessionRequest(string? ShortDescription, string? StaffNotes);
