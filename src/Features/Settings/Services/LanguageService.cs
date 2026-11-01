using System.Globalization;
using Contracts;
using Settings.Dto;
using Settings.Models.AppState;

namespace Settings.Services;

internal sealed class LanguageService(IWritableOptions<AppSettings> settings, ILocalizationSource localization) : ILanguageService
{
    public AppLanguage CurrentLanguage { get; private set; }
        = LoadAndApply(settings);

    public void SetLanguage(AppLanguage language)
    {
        if (CurrentLanguage == language)
            return;

        CurrentLanguage = language;
        ApplyCulture(language);
        localization.Refresh();
        settings.Update(s => s.Language = language);
    }

    private static AppLanguage LoadAndApply(IWritableOptions<AppSettings> settings)
    {
        var language = settings.Value.Language;
        ApplyCulture(language);
        return language;
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
