using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace MyCarDbView.Localizations;

public sealed class LocalizationSource : ILocalizationSource
{
    public static LocalizationSource Instance { get; } = new();

    private readonly Dictionary<string, LocalizedString> _viewCache = new();
    private readonly Dictionary<string, LocalizedString> _dbCache = new();

    public event Action? Refreshed;

    private LocalizationSource() { }

    public LocalizedString Get(string key) => GetOrAdd(_viewCache, key, Localizer.View);

    public LocalizedString GetDb(string key) => GetOrAdd(_dbCache, key, Localizer.Db);

    public void Refresh()
    {
        foreach (var ls in _viewCache.Values)
            ls.Value = Localizer.View[ls.Key].Value;
        foreach (var ls in _dbCache.Values)
            ls.Value = Localizer.Db[ls.Key].Value;
        Refreshed?.Invoke();
    }

    private static LocalizedString GetOrAdd(
        Dictionary<string, LocalizedString> cache,
        string key,
        IStringLocalizer localizer)
    {
        if (cache.TryGetValue(key, out var ls))
            return ls;

        ls = new LocalizedString(key)
        {
            Value = localizer[key].Value
        };

        cache[key] = ls;
        return ls;
    }
}
