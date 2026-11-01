using Settings.Models.AppState;

namespace Settings.Services;

public interface ILanguageService
{
    AppLanguage CurrentLanguage { get; }

    void SetLanguage(AppLanguage language);
}
