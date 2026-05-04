using System;
using System.ComponentModel;

namespace MyCarDbView.Localizations;

internal sealed class LocalizedObservable : IObservable<object?>
{
    private readonly LocalizedString _source;

    public LocalizedObservable(LocalizedString source) => _source = source;

    public IDisposable Subscribe(IObserver<object?> observer)
    {
        observer.OnNext(_source.Value);

        PropertyChangedEventHandler handler = (_, _) => observer.OnNext(_source.Value);
        _source.PropertyChanged += handler;

        return new Subscription(_source, handler);
    }

    private sealed class Subscription : IDisposable
    {
        private LocalizedString? _source;
        private PropertyChangedEventHandler? _handler;

        public Subscription(LocalizedString source, PropertyChangedEventHandler handler)
        {
            _source = source;
            _handler = handler;
        }

        public void Dispose()
        {
            if (_source is null || _handler is null)
                return;
            _source.PropertyChanged -= _handler;
            _source = null;
            _handler = null;
        }
    }
}
