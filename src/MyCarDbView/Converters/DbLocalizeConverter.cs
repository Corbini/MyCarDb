using System.Globalization;
using Avalonia.Data.Converters;
using MyCarDbView.Localizations;

namespace MyCarDbView.Converters;

public sealed class DbLocalizeConverter : IValueConverter
{
    public static DbLocalizeConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string key || string.IsNullOrEmpty(key))
            return value;
        return Localizer.Db[key].Value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
