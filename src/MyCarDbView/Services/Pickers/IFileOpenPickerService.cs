using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services.Pickers;

public interface IFileOpenPickerService
{
    Task<string?> PickAsync(FileOpenPickerOptions options);
}
