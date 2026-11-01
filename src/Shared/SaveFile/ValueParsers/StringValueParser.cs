using System.Text;

using SaveFile.Extensions;
using SaveFile.Values;

namespace SaveFile.ValueParsers;

public class StringValueParser : IValueParser<StringValue>
{
    public const ulong TypeId = 0xffeef1e9fd;

    public StringValue Parse(string name, byte[] rawData)
    {
        byte stringLength = rawData[0];

        string newString = Encoding.UTF8.GetString(rawData, 1, stringLength);

        return new StringValue
        {
            Name = name,
            Value = newString
        };
    }

    public byte[] Serialize(StringValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        writer.WriteLengthPrefixedText(value.Value, value.Name);

        return stream.ToArray();
    }
}
