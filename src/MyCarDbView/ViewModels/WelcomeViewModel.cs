using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Models.AppState;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public sealed partial class WelcomeViewModel(IDialogService _dialogService) : ObservableObject, IViewModel
{
    public IReadOnlyList<LastOpenedFile> RecentFiles { get; } =
    [
        new(@"D:\Mock\My Summer Car\save_01.dat", DateTime.Now.AddHours(-2)),
        new(@"D:\Mock\My Summer Car\save_02.dat", DateTime.Now.AddDays(-1)),
        new(@"D:\Mock\My Summer Car\campaign_run.dat", DateTime.Now.AddDays(-5)),
        new(@"D:\Mock\My Summer Car\test_save.dat", DateTime.Now.AddDays(-14)),
    ];

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
