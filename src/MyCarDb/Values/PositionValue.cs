namespace MyCarDb.Values;

public record PositionValue(
    string Name,
    float RotationX,
    float RotationY,
    float RotationZ,
    float RotationW,
    float AxisX,
    float AxisY,
    float AxisZ,
    string Layer) : IFileValue;
