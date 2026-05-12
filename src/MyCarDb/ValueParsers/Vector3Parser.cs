using System.Text;
using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb.ValueParsers;

public class Vector3Parser : IValueParser<Vector3Value>
{
    public const ulong TypeId = 0xff46dc66ec;

    public Vector3Value Parse(string name, byte[] data)
    {
        float x = BitConverter.ToSingle(data, 0);
        float y = BitConverter.ToSingle(data, 4);
        float z = BitConverter.ToSingle(data, 8);
        return new Vector3Value
        {
            Name = name,
            AxisX = x,
            AxisY = y,
            AxisZ = z
        };
    }


    public byte[] Serialize(Vector3Value value)
    {
        MemoryStream stream = new();
        BinaryWriter writer = new(stream);
        writer.WriteTypeId(TypeId);

        writer.Write((float)value.AxisX);
        writer.Write((float)value.AxisY);
        writer.Write((float)value.AxisZ);

        return stream.ToArray();
    }
}
