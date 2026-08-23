namespace MyCarDb.Values;

public record RawValue : IFileValue
{
    public required string Name { get; init; }
    public required ulong TypeId { get; init; }
    public required string RawData { get; init; }
}
