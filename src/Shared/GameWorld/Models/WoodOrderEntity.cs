using System.Numerics;

namespace GameWorld.Models;

public class WoodOrderEntity(
    string id,
    Vector2 position,
    bool isOrdered,
    float wait) : IMapEntity
{
    public string Id { get; } = id;
    public Vector2 Position { get; } = position;
    public bool IsOrdered { get; internal set; } = isOrdered;
    public float Wait { get; internal set; } = wait;
}
