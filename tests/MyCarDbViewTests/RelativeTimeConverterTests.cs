using System.Globalization;
using MyCarDbView.Converters;
using Xunit;

namespace MyCarDbViewTests.Converters;

public class RelativeTimeConverterTests : IDisposable
{
    private readonly CultureInfo _originalCulture;
    private readonly CultureInfo _originalUiCulture;


    public RelativeTimeConverterTests()
    {
        _originalCulture = CultureInfo.CurrentCulture;
        _originalUiCulture = CultureInfo.CurrentUICulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = _originalCulture;
        CultureInfo.CurrentUICulture = _originalUiCulture;
    }

    private static object? Convert(DateTime value) =>
        RelativeTimeConverter.Instance.Convert(
            value, typeof(string), null, CultureInfo.InvariantCulture);

    [Fact]
    public void Convert_Null_ReturnsNull()
    {
        object? result = RelativeTimeConverter.Instance.Convert(
            null, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Null(result);
    }

    [Fact]
    public void Convert_NonDateTime_ReturnsNull()
    {
        object? result = RelativeTimeConverter.Instance.Convert(
            "not a date", typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Null(result);
    }

    [Fact]
    public void Convert_Today_FormatsWithTodayTemplate()
    {
        var todayMorning = DateTime.Today.AddHours(9).AddMinutes(30);

        string? result = (string?)Convert(todayMorning);

        Assert.NotNull(result);
        Assert.Contains("09:30", result);
    }

    [Fact]
    public void Convert_FutureDate_TreatedAsToday()
    {
        var future = DateTime.Today.AddDays(1).AddHours(8);

        string? result = (string?)Convert(future);

        Assert.NotNull(result);
        Assert.Contains("08:00", result);
    }

    [Fact]
    public void Convert_Yesterday_ReturnsYesterdayString()
    {
        var yesterday = DateTime.Today.AddDays(-1).AddHours(15);

        string? result = (string?)Convert(yesterday);

        Assert.Equal("Yesterday", result);
    }

    [Fact]
    public void Convert_ThreeDaysAgo_ReturnsDayOfWeekName()
    {
        var threeDaysAgo = DateTime.Today.AddDays(-3);
        string expected = threeDaysAgo.ToString("dddd", CultureInfo.InvariantCulture);

        string? result = (string?)Convert(threeDaysAgo);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_SixDaysAgo_StillReturnsDayOfWeekName()
    {
        var sixDaysAgo = DateTime.Today.AddDays(-6);
        string expected = sixDaysAgo.ToString("dddd", CultureInfo.InvariantCulture);

        string? result = (string?)Convert(sixDaysAgo);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_SevenDaysAgo_SwitchesToShortDateFormat()
    {
        var sevenDaysAgo = DateTime.Today.AddDays(-7);
        string dayOfWeek = sevenDaysAgo.ToString("dddd", CultureInfo.InvariantCulture);

        string? result = (string?)Convert(sevenDaysAgo);

        Assert.NotEqual(dayOfWeek, result);
    }

    [Fact]
    public void Convert_DateInCurrentYear_AtLeastWeekOld_ReturnsDayMonthFormat()
    {
        var today = DateTime.Today;
        if (today.DayOfYear <= 7)
            return;

        var sameYearOld = today.AddDays(-7);
        string expected = sameYearOld.ToString("d MMM", CultureInfo.InvariantCulture);

        string? result = (string?)Convert(sameYearOld);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_DateInDifferentYear_ReturnsDayMonthYearFormat()
    {
        var lastYear = DateTime.Today.AddYears(-1);
        string expected = lastYear.ToString("d MMM yyyy", CultureInfo.InvariantCulture);

        string? result = (string?)Convert(lastYear);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertBack_Throws_NotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            RelativeTimeConverter.Instance.ConvertBack(
                "anything", typeof(object), null, CultureInfo.InvariantCulture));
    }
}
