using System.Text;

using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class ArrayStringParser : IValueParser<ArrayStringValue>
{
    public const ulong TypeId = 0x53ffeef1e9;
    private readonly byte[] _startValue = [0xfd, 0x00];


    public ArrayStringValue Parse(string name, byte[] data)
    {
        if (!_startValue.SequenceEqual(data[0..2]))
            throw new Exception("Unknown file");

        int count = BitConverter.ToInt32(data, 2);

        List<string> strings = new(count);

        int readingIndex = 6;
        for (int index = 0; index < count; index++)
        {
            byte stringLength = data[readingIndex];
            readingIndex += 1;

            string newString = Encoding.UTF8.GetString(data, readingIndex, stringLength);
            strings.Add(newString);

            readingIndex += stringLength;
        }

        return new ArrayStringValue
        {
            Name = name,
            Value = strings.ToArray()
        };
    }

    public byte[] Serialize(ArrayStringValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        writer.Write(_startValue);
        writer.Write((Int32)value.Value.Length);
        foreach (string entry in value.Value)
        {
            byte[] entryBytes = Encoding.UTF8.GetBytes(entry);
            writer.Write((byte)entryBytes.Length);
            writer.Write(entryBytes);
        }

        return stream.ToArray();
    }
}
