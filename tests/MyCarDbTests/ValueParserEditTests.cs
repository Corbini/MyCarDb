using MyCarDb;
using MyCarDb.Values;
using Xunit;

namespace MyCarDBTests;

public class ValueParserEditTests
{
    private static readonly byte[] _integerType = [0xFF, 0x56, 0x08, 0xA8, 0xE2];
    private static readonly byte[] _floatType = [0xFF, 0x6B, 0xD7, 0x3E, 0x6E];
    private static readonly byte[] _booleanType = [0xFF, 0x9C, 0x7C, 0x4D, 0xAD];
    private static readonly byte[] _stringType = [0xFF, 0xEE, 0xF1, 0xE9, 0xFD];
    private static readonly byte[] _colorType = [0xFF, 0x31, 0x4B, 0xCF, 0x32];
    private static readonly byte[] _vector3Type = [0xFF, 0x46, 0xDC, 0x66, 0xEC];
    private static readonly byte[] _positionType = [0xFF, 0x76, 0xFA, 0x7A, 0x09];
    private static readonly byte[] _arrayStringType = [0x53, 0xFF, 0xEE, 0xF1, 0xE9];
    private static readonly byte[] _dictionaryType = [0x52, 0xFF, 0xEE, 0xF1, 0xE9];
    private static readonly byte[] _unknownType = [0xAB, 0xCD, 0xEF, 0x12, 0x34];

    private static readonly byte[] _arrayStringHeader = [0xFD, 0x00];
    private static readonly byte[] _dictionaryHeader = [0xFD, 0xEE, 0xF1, 0xE9, 0xFD, 0x00, 0x00];
    private static readonly byte[] _positionHeader = [0x04];

    private static T ParseSingle<T>(byte[] file) where T : IFileValue
        => Assert.IsType<T>(Assert.Single(new FileParser().Parse(file)));

    private static byte[] Serialize(IFileValue value)
        => new FileParser().Serialize([value]);

    [Fact]
    public void Edit_Integer_WritesEditedBytes()
    {
        byte[] file = SaveFileBytes.Entry("INT", _integerType, BitConverter.GetBytes(1234));

        var parsed = ParseSingle<IntegerValue>(file);
        Assert.Equal(1234, parsed.Value);

        byte[] result = Serialize(parsed with { Value = -99 });

        Assert.Equal(SaveFileBytes.Entry("INT", _integerType, BitConverter.GetBytes(-99)), result);
    }

    [Fact]
    public void Edit_Float_WritesEditedBytes()
    {
        byte[] file = SaveFileBytes.Entry("FLT", _floatType, BitConverter.GetBytes(1.5f));

        var parsed = ParseSingle<FloatValue>(file);
        Assert.Equal(1.5f, parsed.Value);

        byte[] result = Serialize(parsed with { Value = -0.25f });

        Assert.Equal(SaveFileBytes.Entry("FLT", _floatType, BitConverter.GetBytes(-0.25f)), result);
    }

    [Fact]
    public void Edit_Boolean_WritesEditedBytes()
    {
        byte[] file = SaveFileBytes.Entry("BOOL", _booleanType, [0x00]);

        var parsed = ParseSingle<BooleanValue>(file);
        Assert.False(parsed.Value);

        byte[] result = Serialize(parsed with { Value = true });

        Assert.Equal(SaveFileBytes.Entry("BOOL", _booleanType, [0x01]), result);
    }

    [Fact]
    public void Edit_String_WritesEditedBytes()
    {
        byte[] file = SaveFileBytes.Entry("STR", _stringType, SaveFileBytes.Text("satsuma"));

        var parsed = ParseSingle<StringValue>(file);
        Assert.Equal("satsuma", parsed.Value);

        byte[] result = Serialize(parsed with { Value = "hayosiko" });

        Assert.Equal(SaveFileBytes.Entry("STR", _stringType, SaveFileBytes.Text("hayosiko")), result);
    }

    [Fact]
    public void Edit_StringWithMultiByteCharacters_WritesByteLengthNotCharCount()
    {
        byte[] file = SaveFileBytes.Entry("STR", _stringType, SaveFileBytes.Text("a"));

        var parsed = ParseSingle<StringValue>(file);

        byte[] result = Serialize(parsed with { Value = "żółć" });

        Assert.Equal(SaveFileBytes.Entry("STR", _stringType, SaveFileBytes.Text("żółć")), result);
    }

    [Fact]
    public void Edit_Color_WritesEditedBytes()
    {
        byte[] file = SaveFileBytes.Entry("COL", _colorType, SaveFileBytes.Floats(0.1f, 0.2f, 0.3f, 1f));

        var parsed = ParseSingle<ColorValue>(file);
        Assert.Equal(0.2f, parsed.Green);

        byte[] result = Serialize(parsed with { Green = 0.75f, Alpha = 0.5f });

        Assert.Equal(
            SaveFileBytes.Entry("COL", _colorType, SaveFileBytes.Floats(0.1f, 0.75f, 0.3f, 0.5f)),
            result);
    }

    [Fact]
    public void Edit_Vector3_WritesEditedBytes()
    {
        byte[] file = SaveFileBytes.Entry("VEC", _vector3Type, SaveFileBytes.Floats(1f, 2f, 3f));

        var parsed = ParseSingle<Vector3Value>(file);
        Assert.Equal(2f, parsed.AxisY);

        byte[] result = Serialize(parsed with { AxisY = -8.5f });

        Assert.Equal(
            SaveFileBytes.Entry("VEC", _vector3Type, SaveFileBytes.Floats(1f, -8.5f, 3f)),
            result);
    }

    [Fact]
    public void Edit_Position_WritesEditedBytes()
    {
        byte[] payload = SaveFileBytes.Concat(
            _positionHeader,
            SaveFileBytes.Floats(1f, 2f, 3f, 0f, 0f, 0f, 1f, 1f, 1f, 1f),
            SaveFileBytes.Text("Default"));
        byte[] file = SaveFileBytes.Entry("POS", _positionType, payload);

        var parsed = ParseSingle<PositionValue>(file);
        Assert.Equal(1f, parsed.AxisX);
        Assert.Equal("Default", parsed.Layer);

        byte[] result = Serialize(parsed with { AxisX = 42.5f, AxisZ = -7f, Layer = "Parts" });

        byte[] expected = SaveFileBytes.Entry("POS", _positionType, SaveFileBytes.Concat(
            _positionHeader,
            SaveFileBytes.Floats(42.5f, 2f, -7f, 0f, 0f, 0f, 1f, 1f, 1f, 1f),
            SaveFileBytes.Text("Parts")));

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Edit_ArrayString_WritesEditedBytes()
    {
        byte[] payload = SaveFileBytes.Concat(
            _arrayStringHeader,
            BitConverter.GetBytes(2),
            SaveFileBytes.Text("bolt"),
            SaveFileBytes.Text("nut"));
        byte[] file = SaveFileBytes.Entry("ARR", _arrayStringType, payload);

        var parsed = ParseSingle<ArrayStringValue>(file);
        Assert.Equal<string[]>(["bolt", "nut"], parsed.Value);

        byte[] result = Serialize(parsed with { Value = ["bolt", "washer", "screw"] });

        byte[] expected = SaveFileBytes.Entry("ARR", _arrayStringType, SaveFileBytes.Concat(
            _arrayStringHeader,
            BitConverter.GetBytes(3),
            SaveFileBytes.Text("bolt"),
            SaveFileBytes.Text("washer"),
            SaveFileBytes.Text("screw")));

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Edit_DictionaryStringString_WritesEditedBytes()
    {
        byte[] payload = SaveFileBytes.Concat(
            _dictionaryHeader,
            BitConverter.GetBytes(1),
            SaveFileBytes.Text("engine"),
            SaveFileBytes.Text("off"));
        byte[] file = SaveFileBytes.Entry("DICT", _dictionaryType, payload);

        var parsed = ParseSingle<DictionaryStringStringValue>(file);
        Assert.Equal("off", parsed.Value["engine"]);

        byte[] result = Serialize(parsed with
        {
            Value = new Dictionary<string, string> { ["engine"] = "running" }
        });

        byte[] expected = SaveFileBytes.Entry("DICT", _dictionaryType, SaveFileBytes.Concat(
            _dictionaryHeader,
            BitConverter.GetBytes(1),
            SaveFileBytes.Text("engine"),
            SaveFileBytes.Text("running")));

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Edit_RawValueOfUnknownType_WritesEditedBytes()
    {
        byte[] file = SaveFileBytes.Entry("UNKNOWN", _unknownType, [0x01, 0x02, 0x03]);

        var parsed = ParseSingle<RawValue>(file);
        Assert.Equal(0xABCDEF1234ul, parsed.TypeId);
        Assert.Equal("010203", parsed.RawData);

        byte[] result = Serialize(parsed with { RawData = "0AFF10" });

        Assert.Equal(SaveFileBytes.Entry("UNKNOWN", _unknownType, [0x0A, 0xFF, 0x10]), result);
    }
}
