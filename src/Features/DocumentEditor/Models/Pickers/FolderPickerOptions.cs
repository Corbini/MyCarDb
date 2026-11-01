namespace DocumentEditor.Models.Pickers;

public sealed record FolderPickerOptions
{
    public string? Title { get; init; }

    public string? SuggestedFolder { get; init; }
}
