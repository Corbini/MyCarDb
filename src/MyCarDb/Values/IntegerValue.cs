namespace MyCarDb.Values;

public record IntegerValue : IFileValue
{
    public required string Name { get; init; }
    public required int Value { get; set; }
}
