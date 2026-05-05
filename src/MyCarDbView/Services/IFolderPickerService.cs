using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services;

public interface IFolderPickerService
{
    Task<string?> PickAsync(FolderPickerOptions options);
}
