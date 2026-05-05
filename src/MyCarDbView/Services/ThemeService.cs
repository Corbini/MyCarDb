using Avalonia;
using Avalonia.Styling;
using MyCarDbView.Dto;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

internal sealed class ThemeService(IWritableOptions<AppSettings> settings) : IThemeService
{
    public AppTheme CurrentTheme { get; private set; } = LoadAndApply(settings);

    public void SetTheme(AppTheme theme)
    {
        if (CurrentTheme == theme)
            return;

        CurrentTheme = theme;
        ApplyTheme(theme);
        settings.Update(s => s.Theme = theme);
    }

    private static AppTheme LoadAndApply(IWritableOptions<AppSettings> settings)
    {
        var theme = settings.Value.Theme;
        ApplyTheme(theme);
        return theme;
    }

    private static void ApplyTheme(AppTheme theme)
    {
        if (Application.Current is null)
            return;

        Application.Current.RequestedThemeVariant = theme switch
        {
            AppTheme.Light => ThemeVariant.Light,
            AppTheme.Dark => ThemeVariant.Dark,
            _ => ThemeVariant.Default,
        };
    }
}
