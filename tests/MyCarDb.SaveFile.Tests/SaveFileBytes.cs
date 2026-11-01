using System.Text;

namespace SaveFile.Tests;

internal static class SaveFileBytes
{
    public static byte[] Entry(string title, byte[] type, byte[] payload)
    {
        byte[] titleBytes = Encoding.UTF8.GetBytes(title);

        var result = new List<byte> { 0x7e, (byte)titleBytes.Length };
        result.AddRange(titleBytes);
        result.AddRange(BitConverter.GetBytes(6 + payload.Length));
        result.AddRange(type);
        result.AddRange(payload);
        result.Add(0x7b);

        return result.ToArray();
    }

    public static byte[] Text(string text)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        return [(byte)textBytes.Length, .. textBytes];
    }

    public static byte[] Floats(params float[] values)
        => values.SelectMany(BitConverter.GetBytes).ToArray();

    public static byte[] Concat(params byte[][] parts)
        => parts.SelectMany(part => part).ToArray();
}
