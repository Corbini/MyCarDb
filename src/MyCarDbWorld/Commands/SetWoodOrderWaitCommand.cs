using MyCarDbWorld.Models;

namespace MyCarDbWorld.Commands;

public sealed class SetWoodOrderWaitCommand(
    string entityId,
    float oldValue,
    float newValue) : IEditCommand
{
    public string DisplayNameKey => "Editor_Command_SetWoodOrderWait";

    public void Apply(World world)
    {
        var entity = world.Entities.OfType<WoodOrderEntity>().First(e => e.Id == entityId);
        entity.Wait = newValue;
    }

    public void Revert(World world)
    {
        var entity = world.Entities.OfType<WoodOrderEntity>().First(e => e.Id == entityId);
        entity.Wait = oldValue;
    }
}
