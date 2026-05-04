using MyCarDbView.Models.AppState;

namespace MyCarDbView.Dto;

public sealed class RecentFiles
{
    public List<LastOpenedFile> Files { get; set; } = [];
}
