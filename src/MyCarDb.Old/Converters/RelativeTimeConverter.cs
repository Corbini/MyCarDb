using System.Globalization;
using Avalonia.Data.Converters;
using MyCarDbView.Localizations;

namespace MyCarDbView.Converters;

public sealed class RelativeTimeConverter : IValueConverter
{
    public static RelativeTimeConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTime dt)
            return null;

        var currentCulture = CultureInfo.CurrentCulture;
        var today = DateTime.Today;
        var date = dt.Date;
        int days = (today - date).Days;

        if (days <= 0)
            return string.Format(
                currentCulture,
                Localizer.View["Welcome_RelativeTime_Today"].Value,
                dt.ToString("HH:mm", currentCulture));
        if (days == 1)
            return Localizer.View["Welcome_RelativeTime_Yesterday"].Value;
        if (days < 7)
            return dt.ToString("dddd", currentCulture);
        return dt.Year == today.Year
            ? dt.ToString("d MMM", currentCulture)
            : dt.ToString("d MMM yyyy", currentCulture);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
