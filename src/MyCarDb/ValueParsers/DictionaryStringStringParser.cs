using System.Text;
using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public class DictionaryStringStringParser : IValueParser<DictionaryStringStringValue>
{
    public const ulong TypeId = 0x52ffeef1e9;

    public DictionaryStringStringValue Parse(string name, byte[] data)
    {
        var test = BitConverter.ToInt32(data, 3);
        var dictionaryAmount = BitConverter.ToInt32(data, 7);

        Dictionary<string, string> dictionary = new(dictionaryAmount);
        var readingIndex = 11;
        for (int index = 0; index < dictionaryAmount; index++)
        {
            var stringLength = data[readingIndex];
            readingIndex += 1;

            var key = Encoding.UTF8.GetString(data, readingIndex, stringLength);
            readingIndex += stringLength;

            var valueLength = data[readingIndex];
            readingIndex += 1;

            var value = Encoding.UTF8.GetString(data, readingIndex, valueLength);
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