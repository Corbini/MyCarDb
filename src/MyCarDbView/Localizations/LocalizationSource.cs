using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace MyCarDbView.Localizations;

public sealed class LocalizationSource
{
    public static LocalizationSource Instance { get; } = new();

    private readonly Dictionary<string, LocalizedString> _viewCache = new();
    private readonly Dictionary<string, LocalizedString> _dbCache = new();

    private LocalizationSource() { }

    public LocalizedString Get(string key) => GetOrAdd(_viewCache, key, Localizer.View);

    public LocalizedString GetDb(string key) => GetOrAdd(_dbCache, key, Localizer.Db);

    public void Refresh()
    {
        foreach (var ls in _viewCache.Values)
            ls.RaiseValueChanged();
        foreach (var ls in _dbCache.Values)
            ls.RaiseValueChanged();
    }

    private static LocalizedString GetOrAdd(
        Dictionary<string, LocalizedString> cache,
        string key,
        IStringLocalizer localizer)
    {
        if (!cache.TryGetValue(key, out var ls))
        {
            ls = new LocalizedString(key, localizer);
            cache[key] = ls;
        }
        return ls;
    }
}
