namespace MyCarDb.Values;

public record DictionaryStringStringValue : IFileValue
{
    public required string Name { get; init; }
    public required Dictionary<string, string> Value { get; set; }

    public virtual bool Equals(DictionaryStringStringValue? other)
        => other is not null
        && Name == other.Name
        && Value.Count == other.Value.Count
        && Value.All(kv => other.Value.TryGetValue(kv.Key, out string? v) && v == kv.Value);

    public override int GetHashCode()
        => HashCode.Combine(Name, Value.Count);
}
