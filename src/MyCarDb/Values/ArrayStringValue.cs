namespace MyCarDb.Values;

public record ArrayStringValue(
    string Name,
    string[] Value) : IFileValue;
