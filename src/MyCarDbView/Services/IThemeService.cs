using System.ComponentModel;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

public interface IThemeService : INotifyPropertyChanged
{
    AppTheme CurrentTheme { get; }

    void SetTheme(AppTheme theme);
}
