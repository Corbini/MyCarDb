using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class ColorParser : IValueParser<ColorValue>
{
    public const ulong TypeId = 0xff314bcf32;

    public ColorValue Parse(string name, byte[] data)
    {
        float read = BitConverter.ToSingle(data, 0);
        float green = BitConverter.ToSingle(data, 4);
        float blue = BitConverter.ToSingle(data, 8);
        float alpha = BitConverter.ToSingle(data, 12);

        return new(
            name,
            read,
            green,
            blue,
            alpha);
    }

    public byte[] Serialize(ColorValue value)
    {
        throw new NotImplementedException();
    }
}
