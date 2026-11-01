using System.Numerics;

namespace GameWorld.Data;

public interface IMapDataRegistry
{
    Vector2? PositionOf(string entityId);
}
