using System.Globalization;
using Avalonia.Data.Converters;

namespace MyCarDbView.Converters;

public sealed class FullTimestampConverter : IValueConverter
{
    public static FullTimestampConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is DateTime dt
            ? dt.ToString("dddd, d MMMM yyyy, HH:mm", CultureInfo.CurrentCulture)
            : null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
