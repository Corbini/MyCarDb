using System.ComponentModel;

namespace MyCarDbView.Localizations;

public sealed class LocalizedString(string key) : INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs _valueChangedArgs = new(nameof(Value));

    public string Value => Localizer.View[key].Value;

    public event PropertyChangedEventHandler? PropertyChanged;

    internal void RaiseValueChanged() => PropertyChanged?.Invoke(this, _valueChangedArgs);
}
