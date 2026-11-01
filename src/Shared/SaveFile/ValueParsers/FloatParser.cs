using SaveFile.Extensions;
using SaveFile.Values;

namespace SaveFile.ValueParsers;

public class FloatValueParser : IValueParser<FloatValue>
{
    public const ulong TypeId = 0xff6bd73e6e;

    public FloatValue Parse(string name, byte[] rawData)
    {
        return new FloatValue
        {
            Name = name,
            Value = BitConverter.ToSingle(rawData)
        };
    }

    public byte[] Serialize(FloatValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        writer.Write(value.Value);

        return stream.ToArray();
    }
}
