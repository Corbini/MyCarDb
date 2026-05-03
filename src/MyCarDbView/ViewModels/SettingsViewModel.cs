using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public enum SettingsCategory
{
    General,
    Shortcuts,
    About,
    Licenses,
}

public sealed partial class SettingsViewModel : ObservableObject, IViewModel
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

    [RelayCommand]
    private void SelectCategory(SettingsCategory category) => SelectedCategory = category;
}
