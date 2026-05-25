using MyCarDb.Values;
using MyCarDbWorld.Coders;
using MyCarDbWorld.Models;

namespace MyCarDbWorld;

public sealed class WorldCoder(
    IEntityCoder<PropertyEntity> propertyCoder,
    IEntityCoder<CesspoolEntity> cesspoolCoder) : IWorldCoder
{
    public World Decode(List<IFileValue> source)
    {
        return new World
        {
            Values = source,
            Entities = [
                .. propertyCoder.Decode(source),
                .. cesspoolCoder.Decode(source)],
        };
    }

    public List<IFileValue> Encode(World destination)
    {
        foreach (var entity in destination.Entities)
        {
            switch (entity)
            {
                case PropertyEntity property:
                    propertyCoder.Encode(property, destination.Values);
                    break;
                case CesspoolEntity cesspool:
                    cesspoolCoder.Encode(cesspool, destination.Values);
                    break;
            }
        }
        return destination.Values;
    }
}
