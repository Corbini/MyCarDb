namespace MyCarDb.Values;

public record FloatValue(
    string Name,
    float Value) : IFileValue;
