using MyCarDb.ValueParsers;
using MyCarDb.Values;

namespace MyCarDb;

public class ValueParserFactory
{
    private readonly ArrayStringParser _arrayStringParser = new();
    private readonly BooleanValueParser _booleanValueParser = new();
    private readonly ColorParser _colorParser = new();
    private readonly DictionaryStringStringParser _dictionaryStringStringParser = new();
    private readonly FloatValueParser _floatValueParser = new();
    private readonly IntegerValueParser _integerValueParser = new();
    private readonly PositionParser _positionParser = new();
    private readonly RawValueParser _rawValueParser = new();
    private readonly StringValueParser _stringValueParser = new();
    private readonly Vector3Parser _vector3Parser = new();

    public IFileValue Parse(string name, ulong type, byte[] rawData)
    {
        return type switch
        {
            PositionParser.TypeId => _positionParser.Parse(name, rawData),
            Vector3Parser.TypeId => _vector3Parser.Parse(name, rawData),
            ArrayStringParser.TypeId => _arrayStringParser.Parse(name, rawData),
            ColorParser.TypeId => _colorParser.Parse(name, rawData),
            DictionaryStringStringParser.TypeId => _dictionaryStringStringParser.Parse(name, rawData),
            IntegerValueParser.TypeId => _integerValueParser.Parse(name, rawData),
            FloatValueParser.TypeId => _floatValueParser.Parse(name, rawData),
            BooleanValueParser.TypeId => _booleanValueParser.Parse(name, rawData),
            StringValueParser.TypeId => _stringValueParser.Parse(name, rawData),
            _ => _rawValueParser.Parse(name, type, rawData)
        };
    }

    public byte[] Serialize(IFileValue value) => value switch
    {
        PositionValue position => _positionParser.Serialize(position),
        Vector3Value vector3 => _vector3Parser.Serialize(vector3),
        ArrayStringValue arrayString => _arrayStringParser.Serialize(arrayString),
        ColorValue color => _colorParser.Serialize(color),
        DictionaryStringStringValue dictionaryStringString => _dictionaryStringStringParser.Serialize(dictionaryStringString),
        IntegerValue integer => _integerValueParser.Serialize(integer),
        FloatValue floatValue => _floatValueParser.Serialize(floatValue),
        BooleanValue boolean => _booleanValueParser.Serialize(boolean),
        StringValue stringValue => _stringValueParser.Serialize(stringValue),
        RawValue raw => _rawValueParser.Serialize(raw),
        _ => throw new NotImplementedException()
    };
}
