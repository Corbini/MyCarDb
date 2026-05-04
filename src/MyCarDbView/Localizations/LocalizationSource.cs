using System.Collections.Generic;

namespace MyCarDbView.Localizations;

public sealed class LocalizationSource
{
    public static LocalizationSource Instance { get; } = new();

    private readonly Dictionary<string, LocalizedString> _cache = new();

    private LocalizationSource() { }

    public LocalizedString Get(string key)
    {
        if (!_cache.TryGetValue(key, out var ls))
        {
            ls = new LocalizedString(key);
            _cache[key] = ls;
        }
        return ls;
    }

    public void Refresh()
    {
        foreach (var ls in _cache.Values)
            ls.RaiseValueChanged();
    }
}
