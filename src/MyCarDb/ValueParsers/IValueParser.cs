using MyCarDB.Values;
namespace MyCarDB.ValueParsers;

public interface IValueParser<T> where T : IFileValue
{
    public T Parse(string name, byte[] data);
    public byte[] Serialize(T value);
}
