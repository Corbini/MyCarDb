using DocumentEditor.Models.Pickers;

namespace DocumentEditor.Services.Pickers;

public interface IFilesOpenPickerService
{
    Task<IReadOnlyList<string>> PickAsync(FileOpenPickerOptions options);
}
