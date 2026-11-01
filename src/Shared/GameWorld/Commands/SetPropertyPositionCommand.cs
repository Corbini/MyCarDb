using System.Numerics;
using GameWorld.Models;

namespace GameWorld.Commands;

public sealed class SetPropertyPositionCommand(
    string entityId,
    Vector2 oldPosition,
    Vector2 newPosition) : IEditCommand
{
    public string DisplayNameKey => "Editor_Command_SetPropertyPosition";

    public void Apply(World world)
    {
        var entity = world.Entities.OfType<PropertyEntity>().First(e => e.Id == entityId);
        entity.Position = newPosition;
    }

    public void Revert(World world)
    {
        var entity = world.Entities.OfType<PropertyEntity>().First(e => e.Id == entityId);
        entity.Position = oldPosition;
    }
}
