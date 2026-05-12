using System.Numerics;

namespace MyCarDbWorld.Models;

public class PropertyEntity : IMapEntity
{
    public PropertyEntity(string id, Vector2 position)
    {
        Id = id;
        Position = position;
    }

    public string Id { get; }
    public Vector2 Position { get; internal set; }
}
