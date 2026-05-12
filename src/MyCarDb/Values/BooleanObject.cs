namespace MyCarDb.Values;

public record BooleanValue : IFileValue
{
    public required string Name { get; init; }
    public required bool Value { get; set; }
}
