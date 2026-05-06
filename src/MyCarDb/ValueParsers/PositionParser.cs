using System.Text;

using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class PositionParser : IValueParser<PositionValue>
{
    public const ulong TypeId = 0xff76fa7a09;

    public PositionValue Parse(string name, byte[] data)
    {
        byte amout = data[0];

        float axisX = BitConverter.ToSingle(data, 1);
        float axisY = BitConverter.ToSingle(data, 5);
        float axisZ = BitConverter.ToSingle(data, 9);

        float rotationX = BitConverter.ToSingle(data, 13);
        float rotationY = BitConverter.ToSingle(data, 17);
        float rotationZ = BitConverter.ToSingle(data, 21);
        float rotationW = BitConverter.ToSingle(data, 25);

        float scaleX = BitConverter.ToSingle(data, 29);
        float scaleY = BitConverter.ToSingle(data, 33);
        float scaleZ = BitConverter.ToSingle(data, 37);

        byte stringLenght = data[41];
        string layer = Encoding.UTF8.GetString(data, 42, stringLenght);

        return new(
            name,
            rotationX,
            rotationY,
            rotationZ,
            rotationW,
            axisX,
            axisY,
            axisZ,
            layer);
    }

    public byte[] Serialize(PositionValue value)
    {
        throw new NotImplementedException();
    }
}
