using System.Numerics;

namespace MyCarDbWorld.Models;

public interface IMapEntity : IEntity
{
    Vector2 Position { get; }
}
