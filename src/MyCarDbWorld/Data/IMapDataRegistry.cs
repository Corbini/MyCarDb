using System.Numerics;

namespace MyCarDbWorld.Data;

public interface IMapDataRegistry
{
    Vector2? PositionOf(string entityId);
}
