using MyCarDbWorld.Models;

namespace MyCarDbWorld.Commands;

public sealed class SetWoodOrderCommand(
    string entityId,
    bool oldValue,
    bool newValue) : IEditCommand
{
    public string DisplayNameKey => "Editor_Command_SetWoodOrder";

    public void Apply(World world)
    {
        var entity = world.Entities.OfType<WoodOrderEntity>().First(e => e.Id == entityId);
        entity.IsOrdered = newValue;
    }

    public void Revert(World world)
    {
        var entity = world.Entities.OfType<WoodOrderEntity>().First(e => e.Id == entityId);
        entity.IsOrdered = oldValue;
    }
}
