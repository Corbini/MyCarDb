using MyCarDB.Extensions;
using MyCarDB.Values;
namespace MyCarDB;

public class FileParser
{
    public List<IFileValue> Read(string filePath)
    {
        var file = File.ReadAllBytes(filePath);

        var stream = new MemoryStream(file);
        var reader = new BinaryReader(stream);

        ValueParserFactory factory = new();

        List<IFileValue> files = new();
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            var firstByte = reader.ReadByte();
            Console.WriteLine(firstByte);

            var titleLenght = reader.ReadByte();
            var title = new string(reader.ReadChars(titleLenght));
            var dataLenght = reader.ReadInt32();
            var type = reader.ReadBytes(5);
            var data = reader.ReadBytes(dataLenght - 6);
            var readEndByte = reader.ReadByte();
            ulong typeId = ((ulong)type[0] << 32) | ((ulong)type[1] << 24) | ((ulong)type[2] << 16) | ((ulong)type[3] << 8) | type[4];

            var newObject = factory.Parse(title, typeId, data);

            Console.WriteLine($"title  = {title}");
            Console.WriteLine($"type = {type.ToHexString()}");
            Console.WriteLine($"data = {data.ToHexString()}");

            files.Add(newObject);
        }


        return files;
    }

    public void Write(string filePath, List<IFileValue> values)
    {
        throw new NotImplementedException();
    }
}
