using GameWorld.Models;

namespace GameWorld.Commands;

public sealed class SetCesspoolLevelCommand(
    string entityId,
    float oldLevel,
    float newLevel) : IEditCommand
{
    public string DisplayNameKey => "Editor_Command_SetCesspoolLevel";

    public void Apply(World world)
    {
        var entity = world.Entities.OfType<CesspoolEntity>().First(e => e.Id == entityId);
        entity.Level = newLevel;
    }

    public void Revert(World world)
    {
        var entity = world.Entities.OfType<CesspoolEntity>().First(e => e.Id == entityId);
        entity.Level = oldLevel;
    }
}
