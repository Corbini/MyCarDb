using GameWorld.Models;

namespace GameWorld.Commands;

public sealed class SetCesspoolRateCommand(
    string entityId,
    float oldRate,
    float newRate) : IEditCommand
{
    public string DisplayNameKey => "Editor_Command_SetCesspoolRate";

    public void Apply(World world)
    {
        var entity = world.Entities.OfType<CesspoolEntity>().First(e => e.Id == entityId);
        entity.Rate = newRate;
    }

    public void Revert(World world)
    {
        var entity = world.Entities.OfType<CesspoolEntity>().First(e => e.Id == entityId);
        entity.Rate = oldRate;
    }
}
