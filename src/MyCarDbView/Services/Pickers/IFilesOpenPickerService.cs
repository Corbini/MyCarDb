using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services.Pickers;

public interface IFilesOpenPickerService
{
    Task<IReadOnlyList<string>> PickAsync(FileOpenPickerOptions options);
}
