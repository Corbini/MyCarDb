using GameWorld.Models;
using SaveFile.Values;

namespace GameWorld.Coders;

public interface IEntityCoder<T> where T : IEntity
{
    IEnumerable<T> Decode(IReadOnlyList<IFileValue> source);

    void Encode(T entity, IReadOnlyList<IFileValue> target);
}
