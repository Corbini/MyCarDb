namespace SaveFile.Values;

public record PositionValue : IFileValue
{
    public required string Name { get; init; }
    public required float RotationX { get; set; }
    public required float RotationY { get; set; }
    public required float RotationZ { get; set; }
    public required float RotationW { get; set; }
    public required float AxisX { get; set; }
    public required float AxisY { get; set; }
    public required float AxisZ { get; set; }
    public required float ScaleX { get; set; }
    public required float ScaleY { get; set; }
    public required float ScaleZ { get; set; }
    public required string Layer { get; set; }
}
