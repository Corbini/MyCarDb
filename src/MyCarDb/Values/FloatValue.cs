namespace MyCarDb.Values;

public record FloatValue : IFileValue
{
    public required string Name { get; init; }
    public required float Value { get; set; }
}
