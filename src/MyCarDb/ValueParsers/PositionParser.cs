using System.Text;
using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public class PositionParser : IValueParser<PositionValue>
{
    public const ulong TypeId = 0xff76fa7a09;

    public PositionValue Parse(string name, byte[] data)
    {
        var amout = data[0];

        var axisX = BitConverter.ToSingle(data, 1);
        var axisY = BitConverter.ToSingle(data, 5);
        var axisZ = BitConverter.ToSingle(data, 9);

        var rotationX = BitConverter.ToSingle(data, 13);
        var rotationY = BitConverter.ToSingle(data, 17);
        var rotationZ = BitConverter.ToSingle(data, 21);
        var rotationW = BitConverter.ToSingle(data, 25);

        var scaleX = BitConverter.ToSingle(data, 29);
        var scaleY = BitConverter.ToSingle(data, 33);
        var scaleZ = BitConverter.ToSingle(data, 37);
    
        var stringLenght = data[41];
        var layer = Encoding.UTF8.GetString(data, 42, stringLenght);

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