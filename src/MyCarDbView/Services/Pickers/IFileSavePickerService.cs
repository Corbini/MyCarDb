using MyCarDbView.Models.Pickers;

namespace MyCarDbView.Services.Pickers;

public interface IFileSavePickerService
{
    Task<string?> PickAsync(FileSavePickerOptions options);
}
