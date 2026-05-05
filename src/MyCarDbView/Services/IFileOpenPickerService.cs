using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services;

public interface IFileOpenPickerService
{
    Task<string?> PickAsync(FileOpenPickerOptions options);
}
