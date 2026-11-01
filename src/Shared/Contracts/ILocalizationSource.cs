namespace Contracts;

public interface ILocalizationSource
{
    LocalizedString Get(string key);

    LocalizedString GetDb(string key);

    event Action? Refreshed;

    void Refresh();
}
