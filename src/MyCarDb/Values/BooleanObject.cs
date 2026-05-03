namespace MyCarDB.Values;

public record BooleanValue(
    string Name,
    bool Value) : IFileValue;
