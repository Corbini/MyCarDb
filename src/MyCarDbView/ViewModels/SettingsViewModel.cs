using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentEditor.Models.AppState;
using DocumentEditor.Services;
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
    IBackupService backupService,
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

    [ObservableProperty]
    private AppLanguage _selectedLanguage = languageService.CurrentLanguage;

    partial void OnSelectedLanguageChanged(AppLanguage value) =>
        languageService.SetLanguage(value);

    [NotifyPropertyChangedFor(nameof(IsSystemTheme))]
    [NotifyPropertyChangedFor(nameof(IsLightTheme))]
    [NotifyPropertyChangedFor(nameof(IsDarkTheme))]
    [ObservableProperty]
    private AppTheme _selectedTheme = themeService.CurrentTheme;

    partial void OnSelectedThemeChanged(AppTheme value) =>
        themeService.SetTheme(value);

    public bool IsSystemTheme => SelectedTheme == AppTheme.System;
    public bool IsLightTheme => SelectedTheme == AppTheme.Light;
    public bool IsDarkTheme => SelectedTheme == AppTheme.Dark;

    [ObservableProperty]
    private bool _isAutoBackupEnabled = backupService.BackupSettings.IsAutoBackupEnabled;

    [ObservableProperty]
    private bool _isFileMonitorEnabled = backupService.BackupSettings.IsFileMonitorEnabled;

    partial void OnIsAutoBackupEnabledChanged(bool value) => backupService.SetAutoBackup(value);

    partial void OnIsFileMonitorEnabledChanged(bool value) => backupService.SetFileMonitor(value);

    [RelayCommand]
    private void SelectCategory(SettingsCategory category) => SelectedCategory = category;

    [RelayCommand]
    private void SelectTheme(AppTheme theme) => SelectedTheme = theme;

    [RelayCommand]
    private void Close()
        => dialogService.Close();
}
