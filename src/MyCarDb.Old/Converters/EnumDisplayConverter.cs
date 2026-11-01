using System.Globalization;
using Avalonia.Data.Converters;
using MyCarDbView.Localizations;

namespace MyCarDbView.Converters;

public sealed class EnumDisplayConverter : IValueConverter
{
    public static EnumDisplayConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return null;
        var type = value.GetType();
        if (!type.IsEnum)
            return value.ToString();

        string prefix = parameter as string ?? type.Name;
        string key = $"{prefix}_{value}";
        return Localizer.View[key].Value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
