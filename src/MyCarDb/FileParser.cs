using System;
using System.Text;
using MyCarDb.Extensions;
using MyCarDb.Values;

namespace MyCarDb;

public class FileParser : IFileParser
{
    private readonly ValueParserFactory _factory = new();
    private readonly byte _firstByte = 0x7e;
    private readonly byte _endByte = 0x7b;

    public List<IFileValue> Parse(byte[] fileContent)
    {
        var stream = new MemoryStream(fileContent);
        var reader = new BinaryReader(stream);

        List<IFileValue> files = new();
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            if (reader.ReadByte() != _firstByte)
                throw new Exception("Unknown file");


            byte titleLength = reader.ReadByte();
            char[] titleBytes = reader.ReadChars(titleLength);
            string title = new string(titleBytes);
            int dataLength = reader.ReadInt32();
            byte[] type = reader.ReadBytes(5);
            byte[] data = reader.ReadBytes(dataLength - 6);

            if (reader.ReadByte() != _endByte)
                throw new Exception("Unknown file");

            ulong typeId = ((ulong)type[0] << 32) | ((ulong)type[1] << 24) | ((ulong)type[2] << 16) | ((ulong)type[3] << 8) | type[4];

            var newObject = _factory.Parse(title, typeId, data);

            files.Add(newObject);
        }


        return files;
    }

    public byte[] Serialize(List<IFileValue> values)
    {
        var stream = new MemoryStream();
        var writer = new BinaryWriter(stream);

        foreach (var value in values)
        {
            writer.Write(_firstByte);

            byte[] titleBytes = Encoding.UTF8.GetBytes(value.Name);
            writer.Write((byte)titleBytes.Length);
            writer.Write(titleBytes);

            byte[] valueData = _factory.Serialize(value);
            writer.Write((Int32)valueData.Length + 1);
            writer.Write(valueData);

            writer.Write(_endByte);
        }

        return stream.ToArray();
    }
}
