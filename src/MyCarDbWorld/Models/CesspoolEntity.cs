using System.Numerics;

namespace MyCarDbWorld.Models;

public class CesspoolEntity(
    string id,
    Vector2 position,
    float level,
    float rate,
    bool isPickupOrdered) : IMapEntity
{
    public string Id { get; } = id;
    public Vector2 Position { get; } = position;
    public float Level { get; internal set; } = level;
    public float Rate { get; internal set; } = rate;
    public bool IsPickupOrdered { get; internal set; } = isPickupOrdered;
}
