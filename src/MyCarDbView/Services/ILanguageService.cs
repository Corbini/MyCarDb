using System.ComponentModel;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

public interface ILanguageService : INotifyPropertyChanged
{
    AppLanguage CurrentLanguage { get; }

    void SetLanguage(AppLanguage language);
}
