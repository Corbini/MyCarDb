using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class ColorParser : IValueParser<ColorValue>
{
    public const ulong TypeId = 0xff314bcf32;

    public ColorValue Parse(string name, byte[] data)
    {
        float red = BitConverter.ToSingle(data, 0);
        float green = BitConverter.ToSingle(data, 4);
        float blue = BitConverter.ToSingle(data, 8);
        float alpha = BitConverter.ToSingle(data, 12);

        return new(
            name,
            red,
            green,
            blue,
            alpha);
    }

    public byte[] Serialize(ColorValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        writer.Write((float)value.Red);
        writer.Write((float)value.Green);
        writer.Write((float)value.Blue);
        writer.Write((float)value.Alpha);

        return stream.ToArray();
    }
}
