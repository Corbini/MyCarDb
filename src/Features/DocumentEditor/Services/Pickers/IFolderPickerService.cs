using DocumentEditor.Models.Pickers;

namespace DocumentEditor.Services.Pickers;

public interface IFolderPickerService
{
    Task<string?> PickAsync(FolderPickerOptions options);
}
