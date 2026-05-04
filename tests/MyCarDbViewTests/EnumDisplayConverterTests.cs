using System.Globalization;
using MyCarDbView.Converters;
using Xunit;

namespace MyCarDbViewTests.Converters;

public class EnumDisplayConverterTests
{
    private enum FakeEnum { Foo, Bar }

    [Fact]
    public void Convert_Null_ReturnsNull()
    {
        object? result = EnumDisplayConverter.Instance.Convert(
            null, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Null(result);
    }

    [Fact]
    public void Convert_NonEnum_ReturnsToStringOfValue()
    {
        object? result = EnumDisplayConverter.Instance.Convert(
            42, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Equal("42", result);
    }

    [Fact]
    public void Convert_EnumWithoutParameter_BuildsKeyAsTypeName_UnderscoreValue()
    {
        object? result = EnumDisplayConverter.Instance.Convert(
            FakeEnum.Foo, typeof(string), null, CultureInfo.InvariantCulture);

        Assert.Equal("FakeEnum_Foo", result);
    }

    [Fact]
    public void Convert_EnumWithStringParameter_UsesParameterAsPrefix()
    {
        object? result = EnumDisplayConverter.Instance.Convert(
            FakeEnum.Bar, typeof(string), "Custom", CultureInfo.InvariantCulture);

        Assert.Equal("Custom_Bar", result);
    }

    [Fact]
    public void ConvertBack_Throws_NotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            EnumDisplayConverter.Instance.ConvertBack(
                "anything", typeof(object), null, CultureInfo.InvariantCulture));
    }
}
