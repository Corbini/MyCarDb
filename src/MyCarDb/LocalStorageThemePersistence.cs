using Microsoft.JSInterop;
using StylishDesign;

namespace MyCarDb;

internal sealed class LocalStorageThemePersistence(IJSRuntime js) : IThemePersistence
{
    private const string _storageKey = "mycardb.theme";

    public async Task SaveAsync(Theme theme)
    {
        if (theme == Theme.System)
            await js.InvokeVoidAsync("localStorage.removeItem", _storageKey);
        else
            await js.InvokeVoidAsync("localStorage.setItem", _storageKey, theme.ToString().ToLowerInvariant());
    }
}
