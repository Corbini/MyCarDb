using MyCarDb.Values;

namespace MyCarDb;

public interface IFileParser
{
    public List<IFileValue> Parse(byte[] fileContent);
    public byte[] Serialize(IReadOnlyList<IFileValue> values);
}
