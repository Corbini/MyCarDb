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
        return new(name, x, y, z);
    }


    public byte[] Serialize(Vector3Value value)
    {
        throw new NotImplementedException();
    }
}
