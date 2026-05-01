using System.Text;
using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public class StringValueParser : IValueParser<StringValue>
{
    public const ulong TypeId = 0xffeef1e9fd;

    public StringValue Parse(string name, byte[] rawData)
    {
        return new(name, Encoding.UTF8.GetString(rawData.Skip(1).ToArray()));
    }

    public byte[] Serialize(StringValue value)
    {
        throw new NotImplementedException();
    }
}