using System.Text;
using MyCarDB;
using MyCarDB.Values;
using Xunit;

namespace MyCarDBTests;

public class FileParserTests
{
    [Fact]
    public void Parse_ShouldReadIntegerAndPositionEntries()
    {
        byte[] bytes = BuildFileBytes(
            BuildEntry(0x7E, "INT", [0xFF, 0x56, 0x08, 0xA8, 0xE2], BitConverter.GetBytes(1234)),
            BuildEntry(0x7E, "POS", [0xFF, 0x76, 0xFA, 0x7A, 0x09], BuildPositionRawData(
                axisX: 1.0f,
                axisY: 2.0f,
                axisZ: 3.0f,
                rotationX: 4.0f,
                rotationY: 5.0f,
                rotationZ: 6.0f,
                rotationW: 7.0f,
                scaleX: 8.0f,
                scaleY: 9.0f,
                scaleZ: 10.0f,
                layer: "Layer")));

        string tempFile = Path.Combine(Path.GetTempPath(), "MyCarDB_FileParserTests.bin");
        File.WriteAllBytes(tempFile, bytes);

        var parser = new FileParser();
        var result = parser.Read(tempFile);

        Assert.Equal(2, result.Count);

        var intObj = Assert.IsType<IntegerValue>(result[0]);
        Assert.Equal("INT", intObj.Name);
        Assert.Equal(1234, intObj.Value);

        var posObj = Assert.IsType<PositionValue>(result[1]);
        Assert.Equal("POS", posObj.Name);
        Assert.Equal(4.0f, posObj.RotationX);
        Assert.Equal(5.0f, posObj.RotationY);
        Assert.Equal(6.0f, posObj.RotationZ);
        Assert.Equal(7.0f, posObj.RotationW);
        Assert.Equal(1.0f, posObj.AxisX);
        Assert.Equal(2.0f, posObj.AxisY);
        Assert.Equal(3.0f, posObj.AxisZ);
        Assert.Equal("Layer", posObj.Layer);
    }

    [Fact]
    public void Parse_ShouldParseRealSaveFiles()
    {
        var parser = new FileParser();

        // Test carparts.txt
        string carpartsPath = Path.Combine(
            AppContext.BaseDirectory,
            "test_data",
            "carparts.txt"
        );
        var carpartsResult = parser.Read(carpartsPath);
        Assert.NotEmpty(carpartsResult);
        Assert.All(carpartsResult, item => Assert.NotNull(item.Name));

        // Test savefile.txt
        string savefilePath = Path.Combine(
            AppContext.BaseDirectory,
            "test_data",
            "savefile.txt"
        );
        var savefileResult = parser.Read(savefilePath);
        Assert.NotEmpty(savefileResult);
        Assert.All(savefileResult, item => Assert.NotNull(item.Name));
    }

    private static byte[] BuildEntry(byte firstByte, string title, byte[] type, byte[] rawData, byte endByte = 0x00)
    {
        byte[] titleBytes = Encoding.UTF8.GetBytes(title);
        int dataLength = 6 + rawData.Length;

        var result = new List<byte> { firstByte, (byte)titleBytes.Length };
        result.AddRange(titleBytes);
        result.AddRange(BitConverter.GetBytes(dataLength));
        result.AddRange(type);
        result.AddRange(rawData);
        result.Add(endByte);
        return result.ToArray();
    }

    private static byte[] BuildFileBytes(params byte[][] entries)
    {
        return entries.SelectMany(x => x).ToArray();
    }

    private static byte[] BuildPositionRawData(
        float axisX,
        float axisY,
        float axisZ,
        float rotationX,
        float rotationY,
        float rotationZ,
        float rotationW,
        float scaleX,
        float scaleY,
        float scaleZ,
        string layer)
    {
        var bytes = new List<byte>();
        bytes.Add(0x01);
        bytes.AddRange(BitConverter.GetBytes(axisX));
        bytes.AddRange(BitConverter.GetBytes(axisY));
        bytes.AddRange(BitConverter.GetBytes(axisZ));
        bytes.AddRange(BitConverter.GetBytes(rotationX));
        bytes.AddRange(BitConverter.GetBytes(rotationY));
        bytes.AddRange(BitConverter.GetBytes(rotationZ));
        bytes.AddRange(BitConverter.GetBytes(rotationW));
        bytes.AddRange(BitConverter.GetBytes(scaleX));
        bytes.AddRange(BitConverter.GetBytes(scaleY));
        bytes.AddRange(BitConverter.GetBytes(scaleZ));

        byte[] layerBytes = Encoding.UTF8.GetBytes(layer);
        bytes.Add((byte)layerBytes.Length);
        bytes.AddRange(layerBytes);

        return bytes.ToArray();
    }
}
