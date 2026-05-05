using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

public interface IThemeService
{
    AppTheme CurrentTheme { get; }

    void SetTheme(AppTheme theme);
}
