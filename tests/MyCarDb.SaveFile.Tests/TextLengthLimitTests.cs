using SaveFile;
using SaveFile.Values;
using Xunit;

namespace SaveFile.Tests;

public class TextLengthLimitTests
{
    private static readonly byte[] _stringType = [0xFF, 0xEE, 0xF1, 0xE9, 0xFD];

    [Fact]
    public void Serialize_TextOf255Bytes_IsWritten()
    {
        string text = new('a', 255);

        byte[] result = new FileParser().Serialize([new StringValue { Name = "STR", Value = text }]);

        Assert.Equal(SaveFileBytes.Entry("STR", _stringType, SaveFileBytes.Text(text)), result);
    }

    [Fact]
    public void Serialize_TextOf256Bytes_ThrowsAndNamesTheEntry()
    {
        var value = new StringValue { Name = "PlayerName", Value = new string('a', 256) };

        var exception = Assert.Throws<ArgumentException>(() => new FileParser().Serialize([value]));

        Assert.Contains("PlayerName", exception.Message);
        Assert.Contains("256", exception.Message);
    }

    [Fact]
    public void Serialize_MultiByteTextOver255BytesButUnder256Chars_Throws()
    {
        var value = new StringValue { Name = "STR", Value = new string('ą', 128) };

        Assert.Throws<ArgumentException>(() => new FileParser().Serialize([value]));
    }

    [Fact]
    public void Serialize_ArrayStringEntryOver255Bytes_Throws()
    {
        var value = new ArrayStringValue { Name = "ARR", Value = ["short", new string('a', 256)] };

        Assert.Throws<ArgumentException>(() => new FileParser().Serialize([value]));
    }

    [Fact]
    public void Serialize_DictionaryKeyOver255Bytes_Throws()
    {
        var value = new DictionaryStringStringValue
        {
            Name = "DICT",
            Value = new Dictionary<string, string> { [new string('a', 256)] = "ok" }
        };

        Assert.Throws<ArgumentException>(() => new FileParser().Serialize([value]));
    }

    [Fact]
    public void Serialize_DictionaryValueOver255Bytes_Throws()
    {
        var value = new DictionaryStringStringValue
        {
            Name = "DICT",
            Value = new Dictionary<string, string> { ["ok"] = new string('a', 256) }
        };

        Assert.Throws<ArgumentException>(() => new FileParser().Serialize([value]));
    }

    [Fact]
    public void Serialize_PositionLayerOver255Bytes_Throws()
    {
        var value = new PositionValue
        {
            Name = "POS",
            AxisX = 1,
            AxisY = 2,
            AxisZ = 3,
            RotationX = 0,
            RotationY = 0,
            RotationZ = 0,
            RotationW = 1,
            ScaleX = 1,
            ScaleY = 1,
            ScaleZ = 1,
            Layer = new string('a', 256)
        };

        Assert.Throws<ArgumentException>(() => new FileParser().Serialize([value]));
    }
}
