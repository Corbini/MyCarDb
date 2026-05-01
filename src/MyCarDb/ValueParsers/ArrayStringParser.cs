using System.Text;
using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public class ArrayStringParser : IValueParser<ArrayStringValue>
{
    public const ulong TypeId = 0x53ffeef1e9;

    public ArrayStringValue Parse(string name, byte[] data)
    {
        var value = BitConverter.ToInt16(data, 0);
        var count = BitConverter.ToInt32(data, 2);

        List<string> strings = new(count);

        var readingIndex = 6;
        for(int index = 0; index < count; index ++)
        {
            var stringLength = data[readingIndex];
            readingIndex += 1;

            var newString = Encoding.UTF8.GetString(data, readingIndex, stringLength);
            strings.Add(newString);

            readingIndex += stringLength;
        }

        return new(name, strings.ToArray());
    }

    public byte[] Serialize(ArrayStringValue value)
    {
        throw new NotImplementedException();
    }
}