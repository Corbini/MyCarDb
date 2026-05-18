using CommunityToolkit.Mvvm.ComponentModel;

namespace MyCarDbView.Localizations;

public sealed class LocalizedString(string key) : ObservableObject
{
    public string Key { get; } = key;

    public string Value
    {
        get;
        internal set => SetProperty(ref field, value);
    } = string.Empty;
}
