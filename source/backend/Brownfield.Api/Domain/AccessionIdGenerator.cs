namespace Brownfield.Api.Domain;

public interface IAccessionIdGenerator
{
    string Generate();
}

public sealed class UuidAccessionIdGenerator : IAccessionIdGenerator
{
    public string Generate() => Guid.NewGuid().ToString("D");
}
