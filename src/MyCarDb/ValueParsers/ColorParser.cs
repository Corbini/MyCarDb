using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public class ColorParser : IValueParser<ColorValue>
{
    public const ulong TypeId = 0xff314bcf32;

    public ColorValue Parse(string name, byte[] data)
    {
        var read = BitConverter.ToSingle(data, 0);
        var green = BitConverter.ToSingle(data, 4);
        var blue = BitConverter.ToSingle(data, 8);
        var alpha = BitConverter.ToSingle(data, 12);

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