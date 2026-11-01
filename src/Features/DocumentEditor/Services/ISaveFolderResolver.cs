namespace DocumentEditor.Services;

public interface ISaveFolderResolver
{
    string? SuggestedFolder { get; }
}
