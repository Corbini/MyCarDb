using Avalonia;
using Avalonia.Markup.Xaml;

namespace MyCarDbView.Localizations;

public sealed class ViewExtension : MarkupExtension
{
    public ViewExtension() { }

    public ViewExtension(string key) => Key = key;

    public string? Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        string.IsNullOrEmpty(Key)
            ? string.Empty
            : new LocalizedObservable(LocalizationSource.Instance.Get(Key)).ToBinding();
}
