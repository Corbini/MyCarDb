namespace MyCarDb.Values;

public record ColorValue(
    string Name,
    float Red,
    float Green,
    float Blue,
    float Alpha
    ) : IFileValue;
