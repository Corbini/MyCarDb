using System.ComponentModel;
using Microsoft.Extensions.Localization;

namespace MyCarDbView.Localizations;

public sealed class LocalizedString(string key, IStringLocalizer localizer) : INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs _valueChangedArgs = new(nameof(Value));

    public LocalizedString(string key) : this(key, Localizer.View) { }

    public string Value => localizer[key].Value;

    public event PropertyChangedEventHandler? PropertyChanged;

    internal void RaiseValueChanged() => PropertyChanged?.Invoke(this, _valueChangedArgs);
}
