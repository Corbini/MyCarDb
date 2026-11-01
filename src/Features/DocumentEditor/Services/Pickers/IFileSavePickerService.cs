using DocumentEditor.Models.Pickers;

namespace DocumentEditor.Services.Pickers;

public interface IFileSavePickerService
{
    Task<string?> PickAsync(FileSavePickerOptions options);
}
