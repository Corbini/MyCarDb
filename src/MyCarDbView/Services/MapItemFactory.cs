using System.Numerics;
using GameWorld.Models;
using MyCarDbView.Items;

namespace MyCarDbView.Services;

public sealed class MapItemFactory : IMapItemFactory
{
    private static readonly Matrix3x2 _worldToCanvas =
        Matrix3x2.CreateScale(1f / 3.9f, -1f / 3.2f)
        * Matrix3x2.CreateTranslation(442f, 546f);

    public MapItem? Create(EntityItem item)
    {
        if (item.Value is not IMapEntity mapEntity)
            return null;

        var pos = Vector2.Transform(mapEntity.Position, _worldToCanvas);

        return new MapItem
        {
            Value = item,
            X = pos.X,
            Z = pos.Y,
        };
    }
}
