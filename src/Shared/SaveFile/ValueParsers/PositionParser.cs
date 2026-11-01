using System.Text;

using SaveFile.Extensions;
using SaveFile.Values;

namespace SaveFile.ValueParsers;

public class PositionParser : IValueParser<PositionValue>
{
    public const ulong TypeId = 0xff76fa7a09;
    private readonly byte _startValue = 0x04;

    public PositionValue Parse(string name, byte[] data)
    {
        if (_startValue != data[0])
            throw new Exception("Unknown file");

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

        return new PositionValue
        {
            Name = name,
            RotationX = rotationX,
            RotationY = rotationY,
            RotationZ = rotationZ,
            RotationW = rotationW,
            AxisX = axisX,
            AxisY = axisY,
            AxisZ = axisZ,
            ScaleX = scaleX,
            ScaleY = scaleY,
            ScaleZ = scaleZ,
            Layer = layer
        };
    }

    public byte[] Serialize(PositionValue value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        writer.Write(_startValue);

        writer.Write(value.AxisX);
        writer.Write(value.AxisY);
        writer.Write(value.AxisZ);

        writer.Write(value.RotationX);
        writer.Write(value.RotationY);
        writer.Write(value.RotationZ);
        writer.Write(value.RotationW);

        writer.Write(value.ScaleX);
        writer.Write(value.ScaleY);
        writer.Write(value.ScaleZ);

        writer.WriteLengthPrefixedText(value.Layer, value.Name);

        return stream.ToArray();
    }
}
