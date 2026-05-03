namespace MyCarDB.Values;

public record RawValue(
    string Name,
    ulong type,
    string RawData
    ) : IFileValue;
