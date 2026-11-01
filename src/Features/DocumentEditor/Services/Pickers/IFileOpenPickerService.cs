using DocumentEditor.Models.Pickers;

namespace DocumentEditor.Services.Pickers;

public interface IFileOpenPickerService
{
    Task<string?> PickAsync(FileOpenPickerOptions options);
}
