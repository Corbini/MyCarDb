using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Localizations;
using MyCarDbView.Models.AppState;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public enum SettingsCategory
{
    General,
    Shortcuts,
    About,
    Licenses,
}

public sealed record ShortcutEntry(string Key, string Shortcut);

public sealed partial class SettingsViewModel(
    ILanguageService languageService,
    IThemeService themeService,
    ILicenseService licenseService,
    IDialogService dialogService) : ObservableObject, IViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGeneralSelected))]
    [NotifyPropertyChangedFor(nameof(IsShortcutsSelected))]
    [NotifyPropertyChangedFor(nameof(IsAboutSelected))]
    [NotifyPropertyChangedFor(nameof(IsLicensesSelected))]
    private SettingsCategory _selectedCategory = SettingsCategory.General;

    public bool IsGeneralSelected => SelectedCategory == SettingsCategory.General;
    public bool IsShortcutsSelected => SelectedCategory == SettingsCategory.Shortcuts;
    public bool IsAboutSelected => SelectedCategory == SettingsCategory.About;
    public bool IsLicensesSelected => SelectedCategory == SettingsCategory.Licenses;

    public IReadOnlyList<LicenseEntry> Licenses => licenseService.Licenses;

    public LicenseEntry OwnLicense => licenseService.OwnLicense;

    public IReadOnlyList<AppLanguage> AvailableLanguages { get; } = Enum.GetValues<AppLanguage>();

    public string AppVersion { get; } = ThisAssembly.Version;

    public AppLanguage SelectedLanguage
    {
        get => languageService.CurrentLanguage;
        set
        {
            if (languageService.CurrentLanguage == value)
                return;
            languageService.SetLanguage(value);
            OnPropertyChanged();
        }
    }

    public AppTheme SelectedTheme
    {
        get => themeService.CurrentTheme;
        set
        {
            if (themeService.CurrentTheme == value)
                return;
            themeService.SetTheme(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsSystemTheme));
            OnPropertyChanged(nameof(IsLightTheme));
            OnPropertyChanged(nameof(IsDarkTheme));
        }
    }

    public bool IsSystemTheme => SelectedTheme == AppTheme.System;
    public bool IsLightTheme => SelectedTheme == AppTheme.Light;
    public bool IsDarkTheme => SelectedTheme == AppTheme.Dark;

    [ObservableProperty]
    private bool _isAutoBackupEnabled = true;

    [ObservableProperty]
    private bool _isFileMonitorEnabled = true;

    [RelayCommand]
    private void SelectCategory(SettingsCategory category) => SelectedCategory = category;

    [RelayCommand]
    private void SelectTheme(AppTheme theme) => SelectedTheme = theme;

    [RelayCommand]
    private void Close()
        => dialogService.Close();
}
