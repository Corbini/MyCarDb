namespace MyCarDB.Values;

public record IntegerValue(
    string Name,
    int Value) : IFileValue;
