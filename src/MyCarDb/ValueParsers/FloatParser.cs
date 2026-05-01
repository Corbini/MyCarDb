using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public class FloatValueParser : IValueParser<FloatValue>
{
    public const ulong TypeId = 0xff6bd73e6e;

    public FloatValue Parse(string name, byte[] rawData)
    {
        return new (name, BitConverter.ToSingle(rawData));
    }

    public byte[] Serialize(FloatValue value)
    {
        throw new NotImplementedException();
    }
}