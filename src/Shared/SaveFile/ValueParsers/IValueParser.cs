using SaveFile.Values;

namespace SaveFile.ValueParsers;

public interface IValueParser<T> where T : IFileValue
{
    public T Parse(string name, byte[] data);
    public byte[] Serialize(T value);
}
