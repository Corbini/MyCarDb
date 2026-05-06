namespace MyCarDb.Values;

public record IntegerValue(
    string Name,
    int Value) : IFileValue;
