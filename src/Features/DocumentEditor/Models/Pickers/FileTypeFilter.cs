namespace DocumentEditor.Models.Pickers;

public sealed record FileTypeFilter
{
    public required string Name { get; init; }

    public IReadOnlyList<string> Patterns { get; init; } = [];
}
