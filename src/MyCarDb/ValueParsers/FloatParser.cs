using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class FloatValueParser : IValueParser<FloatValue>
{
    public const ulong TypeId = 0xff6bd73e6e;

    public FloatValue Parse(string name, byte[] rawData)
    {
        return new(name, BitConverter.ToSingle(rawData));
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
