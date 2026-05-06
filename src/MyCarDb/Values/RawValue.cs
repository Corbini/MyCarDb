namespace MyCarDb.Values;

public record RawValue(
    string Name,
    ulong type,
    string RawData
    ) : IFileValue;
