using DocumentEditor.Models.AppState;

namespace DocumentEditor.Dto;

public sealed class RecentFiles
{
    public List<LastOpenedFile> Files { get; set; } = [];
}
