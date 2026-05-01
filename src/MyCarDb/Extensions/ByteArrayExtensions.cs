using System.Text;

namespace MyCarDB.Extensions;

public static class ByteArrayExtensions
{
    extension(byte[] byteArray)
    {
        public string ToHexString()
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte singleByte in byteArray)
                hex.AppendFormat("{0:x2}", singleByte);
            return hex.ToString();
        }
    }
}
