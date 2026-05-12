using MyCarDb.Values;
using MyCarDbWorld.Coders;
using MyCarDbWorld.Models;

namespace MyCarDbWorld;

public sealed class WorldCoder(
    IEntityCoder<PropertyEntity> propertyCoder) : IWorldCoder
{
    public World Decode(List<IFileValue> source)
    {
        return new World
        {
            Values = source,
            Entities = [
                .. propertyCoder.Decode(source)],
        };
    }

    public List<IFileValue> Encode(World destination)
    {
        return destination.Values;
    }
}
