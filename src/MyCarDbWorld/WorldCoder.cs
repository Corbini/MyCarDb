using MyCarDb.Values;
using MyCarDbWorld.Models;

namespace MyCarDbWorld;

public class WorldCoder : IWorldCoder
{
    public World Decode(List<IFileValue> source)
    {
        return new()
        {
            Values = source
        };
    }

    public List<IFileValue> Encode(World destination)
    {
        return destination.Values;
    }
}
