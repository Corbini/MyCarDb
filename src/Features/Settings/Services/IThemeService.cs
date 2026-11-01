using Settings.Models.AppState;

namespace Settings.Services;

public interface IThemeService
{
    AppTheme CurrentTheme { get; }

    void SetTheme(AppTheme theme);
}
