using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

internal sealed partial class ThemeService : ObservableObject, IThemeService
{
    [ObservableProperty]
    private AppTheme _currentTheme = AppTheme.System;

    public ThemeService() => ApplyTheme(_currentTheme);

    public void SetTheme(AppTheme theme)
    {
        if (CurrentTheme == theme)
            return;
            
        CurrentTheme = theme;
        ApplyTheme(theme);
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
