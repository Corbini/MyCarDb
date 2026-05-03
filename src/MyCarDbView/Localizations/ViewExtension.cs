using System;
using Avalonia.Markup.Xaml;

namespace MyCarDbView.Localizations;

public sealed class ViewExtension : MarkupExtension
{
    public ViewExtension() { }

    public ViewExtension(string key) => Key = key;

    [System.ComponentModel.DefaultValue(null)]
    public string? Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        string.IsNullOrEmpty(Key) ? string.Empty : Localizer.View[Key].Value;
}
