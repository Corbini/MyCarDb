using System.ComponentModel;
using Avalonia;
using Avalonia.Markup.Xaml;
using Contracts;

namespace MyCarDbView.Localizations;

public sealed class ViewExtension : MarkupExtension
{
    public ViewExtension() { }

    public ViewExtension(string key) => Key = key;

    public string? Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        string.IsNullOrEmpty(Key)
            ? string.Empty
            : new LocalizedStringObservable(LocalizationSource.Instance.Get(Key)).ToBinding();

    private sealed class LocalizedStringObservable(LocalizedString source) : IObservable<string>
    {
        public IDisposable Subscribe(IObserver<string> observer)
        {
            observer.OnNext(source.Value);
            var sub = new Subscription(source, observer);
            source.PropertyChanged += sub.OnSourceChanged;
            return sub;
        }

        private sealed class Subscription(LocalizedString source, IObserver<string> observer) : IDisposable
        {
            public void OnSourceChanged(object? sender, PropertyChangedEventArgs e) =>
                observer.OnNext(source.Value);

            public void Dispose() => source.PropertyChanged -= OnSourceChanged;
        }
    }
}
