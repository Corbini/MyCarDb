using MyCarDb.Values;
using MyCarDbWorld.Models;

namespace MyCarDbWorld.Coders;

public interface IEntityCoder<T> where T : IEntity
{
    IEnumerable<T> Decode(IReadOnlyList<IFileValue> source);

    void Encode(T entity, IReadOnlyList<IFileValue> target);
}
