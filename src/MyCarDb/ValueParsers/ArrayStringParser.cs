using System.Text;

using MyCarDb.Values;
namespace MyCarDb.ValueParsers;

public class ArrayStringParser : IValueParser<ArrayStringValue>
{
    public const ulong TypeId = 0x53ffeef1e9;

    public ArrayStringValue Parse(string name, byte[] data)
    {
        short value = BitConverter.ToInt16(data, 0);
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

        return new(name, strings.ToArray());
    }

    public byte[] Serialize(ArrayStringValue value)
    {
        throw new NotImplementedException();
    }
}
