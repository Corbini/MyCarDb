using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Models.AppState;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public sealed partial class WelcomeViewModel(IDialogService _dialogService, ILastOpenedService _lastOpenedService) : ObservableObject, IViewModel
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
    private void OpenFileDialog()
    {
        Console.WriteLine($"Opening file dialog");
    }


    [RelayCommand]
    private void OpenFileFromPath(string path)
    {
        Console.WriteLine($"Opening file from path");
    }
}
