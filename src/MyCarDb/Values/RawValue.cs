namespace MyCarDb.Values;

public record RawValue : IFileValue
{
    public required string Name { get; init; }
    public required ulong type { get; set; }
    public required string RawData { get; set; }
}
