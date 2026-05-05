using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

public interface ILanguageService
{
    AppLanguage CurrentLanguage { get; }

    void SetLanguage(AppLanguage language);
}
