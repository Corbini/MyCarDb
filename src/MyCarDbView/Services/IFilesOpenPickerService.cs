using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services;

public interface IFilesOpenPickerService
{
    Task<IReadOnlyList<string>> PickAsync(FileOpenPickerOptions options);
}
