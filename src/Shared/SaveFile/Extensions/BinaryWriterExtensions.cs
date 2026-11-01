using System.Buffers.Binary;
using System.Text;

namespace SaveFile.Extensions;

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

        public void WriteLengthPrefixedText(string text, string owner)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            if (textBytes.Length > byte.MaxValue)
                throw new ArgumentException(
                    $"Text in '{owner}' is {textBytes.Length} bytes, limit is {byte.MaxValue}.",
                    nameof(text));

            writer.Write((byte)textBytes.Length);
            writer.Write(textBytes);
        }
    }
}
