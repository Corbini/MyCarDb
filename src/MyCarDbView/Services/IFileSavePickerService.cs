using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services;

public interface IFileSavePickerService
{
    Task<string?> PickAsync(FileSavePickerOptions options);
}
