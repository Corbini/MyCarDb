using GameWorld.Models;
using SaveFile.Values;

namespace GameWorld;

public interface IWorldCoder
{
    public World Decode(List<IFileValue> source);

    public IReadOnlyList<IFileValue> Encode(World destination);
}
