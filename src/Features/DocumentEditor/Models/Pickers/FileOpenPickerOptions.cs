namespace DocumentEditor.Models.Pickers;

public sealed record FileOpenPickerOptions
{
    public string? Title { get; init; }

    public string? SuggestedFolder { get; init; }

    public IReadOnlyList<FileTypeFilter> FileTypes { get; init; } = [];
}
