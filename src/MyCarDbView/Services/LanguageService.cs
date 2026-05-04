using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbView.Localizations;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

internal sealed partial class LanguageService : ObservableObject, ILanguageService
{
    [ObservableProperty]
    private AppLanguage _currentLanguage = AppLanguage.System;

    public LanguageService() => ApplyCulture(_currentLanguage);

    public void SetLanguage(AppLanguage language)
    {
        if (CurrentLanguage == language)
            return;
        CurrentLanguage = language;
        ApplyCulture(language);
        LocalizationSource.Instance.Refresh();
    }

    private static void ApplyCulture(AppLanguage language)
    {
        var culture = language switch
        {
            AppLanguage.System => CultureInfo.InstalledUICulture,
            AppLanguage.Polish => new CultureInfo("pl-PL"),
            AppLanguage.English => new CultureInfo("en-US"),
            _ => CultureInfo.InvariantCulture,
        };
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
}
