using System.Text;

using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class DictionaryStringStringParser : IValueParser<DictionaryStringStringValue>
{
    public const ulong TypeId = 0x52ffeef1e9;

    public DictionaryStringStringValue Parse(string name, byte[] data)
    {
        int test = BitConverter.ToInt32(data, 3);
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

        return new(name, dictionary);
    }

    public byte[] Serialize(DictionaryStringStringValue value)
    {
        throw new NotImplementedException();
    }
}
