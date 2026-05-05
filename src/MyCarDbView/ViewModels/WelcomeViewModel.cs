using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using MyCarDbView.Localizations;
using MyCarDbView.Models.AppState;
using MyCarDbView.Models.Pickers;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public sealed partial class WelcomeViewModel(
    IDialogService _dialogService,
    ILastOpenedService _lastOpenedService,
    IFileOpenPickerService _filePicker,
    ISaveFolderResolver _folderResolver,
    IStringLocalizer<ViewLocalization> _viewLoc) : ObservableObject, IViewModel
{
    public IReadOnlyList<LastOpenedFile> RecentFiles => _lastOpenedService.Recent;

    public bool IsRecentEmpty => _lastOpenedService.Recent.Count == 0;

    [ObservableProperty]
    private bool _isSidebarVisible = true;

    [RelayCommand]
    private void ToggleSidebar() => IsSidebarVisible = !IsSidebarVisible;

    [RelayCommand]
    private Task OpenSettingsAsync(CancellationToken ct) =>
        _dialogService.ShowAsync<SettingsViewModel>(ct);

    [RelayCommand]
    private async Task OpenFileDialogAsync()
    {
        string? path = await _filePicker.PickAsync(new FileOpenPickerOptions
        {
            Title = _viewLoc["Welcome_OpenSave_Label"].Value,
            SuggestedFolder = _folderResolver.SuggestedFolder,
            FileTypes = [new FileTypeFilter { Name = _viewLoc["Welcome_OpenSave_FileType"].Value, Patterns = ["*.txt"] }],
        });

        if (path is null)
            return;

        OpenFileFromPath(path);
    }

    [RelayCommand]
    private void OpenFileFromPath(string path)
    {
        // TODO: load the save and open it in the editor; show an error if it fails.
        _lastOpenedService.Add(new LastOpenedFile(path, DateTime.Now));
    }
}
