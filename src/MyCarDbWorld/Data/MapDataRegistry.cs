using System.Numerics;

namespace MyCarDbWorld.Data;

public sealed class MapDataRegistry : IMapDataRegistry
{
    private static readonly IReadOnlyDictionary<string, Vector2> _positions =
        new Dictionary<string, Vector2>
        {
            ["ShitWell01"] = new Vector2(-1315, 1205),
            ["ShitWell02"] = new Vector2(-1259, 1120),
            ["ShitWell03"] = new Vector2(1495, 720),
            ["ShitWell04"] = new Vector2(1554, 656),
            ["ShitWell05"] = new Vector2(1838, -790),
        };

    public Vector2? PositionOf(string entityId) =>
        _positions.TryGetValue(entityId, out var position) ? position : null;
}
