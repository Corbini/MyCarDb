using MyCarDbWorld.Models;

namespace MyCarDbWorld.Commands;

public sealed class SetCesspoolPickupCommand(
    string entityId,
    bool oldValue,
    bool newValue) : IEditCommand
{
    public string DisplayNameKey => "Editor_Command_SetCesspoolPickup";

    public void Apply(World world)
    {
        var entity = world.Entities.OfType<CesspoolEntity>().First(e => e.Id == entityId);
        entity.IsPickupOrdered = newValue;
    }

    public void Revert(World world)
    {
        var entity = world.Entities.OfType<CesspoolEntity>().First(e => e.Id == entityId);
        entity.IsPickupOrdered = oldValue;
    }
}
