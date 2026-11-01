using System.Text;

namespace SaveFile.Extensions;

public static class ByteArrayExtensions
{
    extension(byte[] byteArray)
    {
        public string ToHexString()
        {
            var hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte singleByte in byteArray)
                hex.AppendFormat("{0:x2}", singleByte);
            return hex.ToString();
        }
    }
}
