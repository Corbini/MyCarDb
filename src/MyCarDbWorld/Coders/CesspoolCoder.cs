using System.Numerics;
using MyCarDb.Values;
using MyCarDbWorld.Models;

namespace MyCarDbWorld.Coders;

public sealed class CesspoolCoder : IEntityCoder<CesspoolEntity>
{
    private static readonly string[] _indices = ["01", "02", "03", "04", "05"];

    public IEnumerable<CesspoolEntity> Decode(IReadOnlyList<IFileValue> source)
    {
        foreach (string index in _indices)
        {
            var level = source.OfType<FloatValue>().FirstOrDefault(v => v.Name == $"ShitWellLevel{index}");

            var rate = source.OfType<FloatValue>().FirstOrDefault(v => v.Name == $"ShitWellRate{index}");
            var called = source.OfType<BooleanValue>().FirstOrDefault(v => v.Name == $"ShitWellLevel{index}Called");

            if (level == null || rate == null || called == null)
                continue;

            yield return new CesspoolEntity(
                id: $"ShitWell{index}",
                level: level.Value,
                rate: rate.Value,
                isPickupOrdered: called.Value);
        }
    }

    public void Encode(CesspoolEntity entity, IList<IFileValue> target)
    {
        string index = entity.Id["ShitWell".Length..];

        var level = target.OfType<FloatValue>().FirstOrDefault(v => v.Name == $"ShitWellLevel{index}");
        var rate = target.OfType<FloatValue>().FirstOrDefault(v => v.Name == $"ShitWellRate{index}");
        var called = target.OfType<BooleanValue>().FirstOrDefault(v => v.Name == $"ShitWellLevel{index}Called");

        if (level is null || rate is null || called is null)
            throw new InvalidOperationException($"Missing values for cesspool '{entity.Id}' in target.");

        level.Value = entity.Level;
        rate.Value = entity.Rate;
        called.Value = entity.IsPickupOrdered;
    }
}