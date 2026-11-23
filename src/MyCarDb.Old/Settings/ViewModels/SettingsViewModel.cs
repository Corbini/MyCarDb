using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contracts;
using Settings.Models.AppState;
using Settings.Services;

namespace Settings.ViewModels;

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
    IDialogService dialogService,
    IApplicationInfo applicationInfo,
    IEnumerable<ISettingsSection> sections) : ObservableObject, IViewModel
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

    public IReadOnlyList<ISettingsSection> Sections { get; } = [.. sections];

    public string AppVersion { get; } = applicationInfo.Version;

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

    [RelayCommand]
    private void SelectCategory(SettingsCategory category) => SelectedCategory = category;

    [RelayCommand]
    private void SelectTheme(AppTheme theme) => SelectedTheme = theme;

    [RelayCommand]
    private void Close()
        => dialogService.Close();
}
