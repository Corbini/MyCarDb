using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb;

public class FileParser: IFileParser
{
    public List<IFileValue> Parse(byte[] fileContent)
    {
        var stream = new MemoryStream(fileContent);
        var reader = new BinaryReader(stream);

        ValueParserFactory factory = new();

        List<IFileValue> files = new();
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            byte firstByte = reader.ReadByte();
            Console.WriteLine(firstByte);

            byte titleLenght = reader.ReadByte();
            string title = new string(reader.ReadChars(titleLenght));
            int dataLenght = reader.ReadInt32();
            byte[] type = reader.ReadBytes(5);
            byte[] data = reader.ReadBytes(dataLenght - 6);
            byte readEndByte = reader.ReadByte();
            ulong typeId = ((ulong)type[0] << 32) | ((ulong)type[1] << 24) | ((ulong)type[2] << 16) | ((ulong)type[3] << 8) | type[4];

            var newObject = factory.Parse(title, typeId, data);

            Console.WriteLine($"title  = {title}");
            Console.WriteLine($"type = {type.ToHexString()}");
            Console.WriteLine($"data = {data.ToHexString()}");

            files.Add(newObject);
        }


        return files;
    }

    public byte[] Serialize(List<IFileValue> values)
    {
        throw new NotImplementedException();
    }
}
