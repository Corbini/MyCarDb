namespace MyCarDB.Values;

public record ArrayStringValue(
    string Name,
    string[] Value) : IFileValue;
