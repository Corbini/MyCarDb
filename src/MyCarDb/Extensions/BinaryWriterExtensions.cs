using System.Buffers.Binary;

namespace MyCarDb.Extensions;

public static class BinaryWriterExtensions
{
    extension(BinaryWriter writer)
    {
        public void WriteTypeId(ulong typeId)
        {
            Span<byte> buffer = stackalloc byte[8];
            BinaryPrimitives.WriteUInt64BigEndian(buffer, typeId);
            writer.Write(buffer[3..]);
        }
    }
}
