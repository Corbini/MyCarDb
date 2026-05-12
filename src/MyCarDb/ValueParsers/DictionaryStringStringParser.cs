using System.Text;

using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class DictionaryStringStringParser : IValueParser<DictionaryStringStringValue>
{
    public const ulong TypeId = 0x52ffeef1e9;

    private readonly byte[] _startValue = [0xfd, 0xee, 0xf1, 0xe9, 0xfd, 0x00, 0x00];

    public DictionaryStringStringValue Parse(string name, byte[] data)
    {
        if (!_startValue.SequenceEqual(data[0..7]))
            throw new Exception("Unknown file");

        int dictionaryAmount = BitConverter.ToInt32(data, 7);

        Dictionary<string, string> dictionary = new(dictionaryAmount);
        int readingIndex = 11;
        for (int index = 0; index < dictionaryAmount; index++)
        {
            byte stringLength = data[readingIndex];
            readingIndex += 1;

            string key = Encoding.UTF8.GetString(data, readingIndex, stringLength);
            readingIndex += stringLength;

            byte valueLength = data[readingIndex];
            readingIndex += 1;

            string value = Encoding.UTF8.GetString(data, readingIndex, valueLength);
            readingIndex += valueLength;

            dictionary.Add(key, value);
        }

        return new DictionaryStringStringValue
        {
            Name = name,
            Value = dictionary
        };
    }

    public byte[] Serialize(DictionaryStringStringValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        writer.Write(_startValue);
        writer.Write((Int32)value.Value.Count);
        foreach (var row in value.Value)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(row.Key);
            writer.Write((byte)keyBytes.Length);
            writer.Write(keyBytes);

            byte[] valueBytes = Encoding.UTF8.GetBytes(row.Value);
            writer.Write((byte)valueBytes.Length);
            writer.Write(valueBytes);
        }

        return stream.ToArray();
    }
}
