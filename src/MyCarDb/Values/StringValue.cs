namespace MyCarDb.Values;

public record StringValue(
    string Name,
    string Value
    ) : IFileValue;
