using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class RawValueParser
{
    public RawValue Parse(string name, ulong typeId, byte[] rawData)
    {
        return new RawValue
        {
            Name = name,
            TypeId = typeId,
            RawData = rawData.ToHexString()
        };
    }

    public byte[] Serialize(RawValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(value.TypeId);

        writer.Write(Convert.FromHexString(value.RawData));

        return stream.ToArray();
    }
}
