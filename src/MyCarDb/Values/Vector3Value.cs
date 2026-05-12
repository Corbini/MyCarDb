namespace MyCarDb.Values;

public record Vector3Value : IFileValue
{
    public required string Name { get; init; }
    public required float AxisX { get; set; }
    public required float AxisY { get; set; }
    public required float AxisZ { get; set; }
}
