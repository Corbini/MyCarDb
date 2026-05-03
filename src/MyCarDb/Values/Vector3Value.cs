namespace MyCarDB.Values;

public record Vector3Value(
    string Name,
    float AxisX,
    float AxisY,
    float AxisZ) : IFileValue;

