namespace DocumentEditor.Models.Pickers;

public sealed record FileSavePickerOptions
{
    public string? Title { get; init; }

    public string? SuggestedFolder { get; init; }

    public string? SuggestedFileName { get; init; }

    public IReadOnlyList<FileTypeFilter> FileTypes { get; init; } = [];
}
