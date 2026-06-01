using MyCarDb.Values;
using MyCarDbWorld.Data;
using MyCarDbWorld.Models;

namespace MyCarDbWorld.Coders;

public sealed class WoodOrderCoder(IMapDataRegistry mapData) : IEntityCoder<WoodOrderEntity>
{
    private static readonly int[] _indices = [1, 2, 3, 4];

    public IEnumerable<WoodOrderEntity> Decode(IReadOnlyList<IFileValue> source)
    {
        foreach (int index in _indices)
        {
            string id = $"Wood{index}";
            var order = source.OfType<BooleanValue>().FirstOrDefault(v => v.Name == $"Wood{index}Order");
            var wait = source.OfType<FloatValue>().FirstOrDefault(v => v.Name == $"Wood{index}OrderWait");

            if (order is null || wait is null)
                continue;

            var position = mapData.PositionOf(id)
                ?? throw new InvalidOperationException($"No map position registered for '{id}'.");

            yield return new WoodOrderEntity(
                id: id,
                position: position,
                isOrdered: order.Value,
                wait: wait.Value);
        }
    }

    public void Encode(WoodOrderEntity entity, IList<IFileValue> target)
    {
        string index = entity.Id["Wood".Length..];
        var order = target.OfType<BooleanValue>().FirstOrDefault(v => v.Name == $"Wood{index}Order");
        var wait = target.OfType<FloatValue>().FirstOrDefault(v => v.Name == $"Wood{index}OrderWait");

        if (order is null || wait is null)
            throw new InvalidOperationException($"Missing values for wood order '{entity.Id}' in target.");

        order.Value = entity.IsOrdered;
        wait.Value = entity.Wait;
    }
}
