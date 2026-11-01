namespace SaveFile.Values;

public record ArrayStringValue : IFileValue
{
    public required string Name { get; init; }
    public required string[] Value { get; set; }

    public virtual bool Equals(ArrayStringValue? other)
        => other is not null
        && Name == other.Name
        && Value.SequenceEqual(other.Value);

    public override int GetHashCode()
        => HashCode.Combine(Name, Value.Length);
}
