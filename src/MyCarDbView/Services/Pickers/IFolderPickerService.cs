using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services.Pickers;

public interface IFolderPickerService
{
    Task<string?> PickAsync(FolderPickerOptions options);
}
