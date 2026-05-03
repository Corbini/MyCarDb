using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public class BooleanValueParser : IValueParser<BooleanValue>
{
    public const ulong TypeId = 0xff9c7c4dad;

    public BooleanValue Parse(string name, byte[] rawData)
    {
        return new(name, rawData[0] != 0);
    }

    public byte[] Serialize(BooleanValue value)
    {
        throw new NotImplementedException();
    }
}
