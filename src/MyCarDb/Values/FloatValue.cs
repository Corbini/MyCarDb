namespace MyCarDB.Values;

public record FloatValue(
    string Name,
    float Value) : IFileValue;
