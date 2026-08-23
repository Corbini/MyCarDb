using MyCarDb.Values;
using MyCarDbWorld.Models;

namespace MyCarDbWorld;

public interface IWorldCoder
{
    public World Decode(List<IFileValue> source);

    public IReadOnlyList<IFileValue> Encode(World destination);
}
