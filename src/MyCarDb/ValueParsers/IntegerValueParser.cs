using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class IntegerValueParser : IValueParser<IntegerValue>
{
    public const ulong TypeId = 0xff5608a8e2;

    public IntegerValue Parse(string name, byte[] rawData)
    {
        return new(name, BitConverter.ToInt32(rawData));
    }

    public byte[] Serialize(IntegerValue value)
    {
        throw new NotImplementedException();
    }
}
