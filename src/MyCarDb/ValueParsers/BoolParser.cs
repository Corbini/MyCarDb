using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class BooleanValueParser : IValueParser<BooleanValue>
{
    public const ulong TypeId = 0xff9c7c4dad;

    public BooleanValue Parse(string name, byte[] rawData)
    {
        return new BooleanValue
        {
            Name = name,
            Value = rawData[0] != 0
        };
    }

    public byte[] Serialize(BooleanValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);

        writer.WriteTypeId(TypeId);
        writer.Write(value.Value);

        return stream.ToArray();
    }
}
