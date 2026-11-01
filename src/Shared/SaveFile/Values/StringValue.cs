namespace SaveFile.Values;

public record StringValue : IFileValue
{
    public required string Name { get; init; }
    public required string Value { get; set; }
}
