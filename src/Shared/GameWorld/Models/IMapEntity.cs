using System.Numerics;

namespace GameWorld.Models;

public interface IMapEntity : IEntity
{
    Vector2 Position { get; }
}
