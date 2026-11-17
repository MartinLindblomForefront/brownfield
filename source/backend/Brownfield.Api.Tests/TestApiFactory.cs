using Brownfield.Api.Adapters.Outbound;
using Brownfield.Api.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Brownfield.Api.Tests;

public sealed class TestApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databasePath = Path.Combine(
        Path.GetTempPath(),
        $"brownfield-api-{Guid.NewGuid():N}.db"
    );
    private readonly IAccessionIdGenerator _idGenerator;

    public string ConnectionString => $"Data Source={_databasePath};Pooling=False";

    public TestApiFactory()
        : this(new UuidAccessionIdGenerator()) { }

    internal TestApiFactory(IAccessionIdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<SqliteAccessionStore>();
            services.AddSingleton(new SqliteAccessionStore(ConnectionString));
            services.RemoveAll<IAccessionIdGenerator>();
            services.AddSingleton(_idGenerator);
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            File.Delete(_databasePath);
        }
    }
}
