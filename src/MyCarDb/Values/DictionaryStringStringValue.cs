namespace MyCarDb.Values;

public record DictionaryStringStringValue(
    string Name,
    Dictionary<string, string> Value
    ) : IFileValue;
