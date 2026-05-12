using System.Text;

using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class StringValueParser : IValueParser<StringValue>
{
    public const ulong TypeId = 0xffeef1e9fd;

    public StringValue Parse(string name, byte[] rawData)
    {
        byte stringLength = rawData[0];

        string newString = Encoding.UTF8.GetString(rawData, 1, stringLength);

        return new(name, newString);
    }

    public byte[] Serialize(StringValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        byte[] valueBytes = Encoding.UTF8.GetBytes(value.Value);
        writer.Write((byte)valueBytes.Length);
        writer.Write(valueBytes);

        return stream.ToArray();
    }
}
