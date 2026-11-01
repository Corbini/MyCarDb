using System.Numerics;
using GameWorld.Data;
using GameWorld.Models;
using SaveFile.Values;

namespace GameWorld.Coders;

public sealed class CesspoolCoder(IMapDataRegistry mapData) : IEntityCoder<CesspoolEntity>
{
    private static readonly string[] _indices = ["01", "02", "03", "04", "05"];

    private readonly string _levelNameFormat = "ShitWellLevel{0}";
    private readonly string _rateNameFormat = "ShitWellRate{0}";
    private readonly string _calledNameFormat = "ShitWellLevel{0}Called";

    public IEnumerable<CesspoolEntity> Decode(IReadOnlyList<IFileValue> source)
    {
        foreach (string index in _indices)
        {
            string id = $"ShitWell{index}";

            string levelName = string.Format(_levelNameFormat, index);
            var level = source.OfType<FloatValue>().FirstOrDefault(v => v.Name == levelName);

            string rateName = string.Format(_rateNameFormat, index);
            var rate = source.OfType<FloatValue>().FirstOrDefault(v => v.Name == rateName);

            string calledName = string.Format(_calledNameFormat, index);
            var called = source.OfType<BooleanValue>().FirstOrDefault(v => v.Name == calledName);

            if (level == null || rate == null || called == null)
                continue;

            var position = mapData.PositionOf(id)
                ?? throw new InvalidOperationException($"No map position registered for '{id}'.");

            yield return new CesspoolEntity(
                id: id,
                position: position,
                level: level.Value,
                rate: rate.Value,
                isPickupOrdered: called.Value);
        }
    }

    public void Encode(CesspoolEntity entity, IReadOnlyList<IFileValue> target)
    {
        string index = entity.Id["ShitWell".Length..];

        string levelName = string.Format(_levelNameFormat, index);
        var level = target.OfType<FloatValue>().FirstOrDefault(v => v.Name == levelName);

        string rateName = string.Format(_rateNameFormat, index);
        var rate = target.OfType<FloatValue>().FirstOrDefault(v => v.Name == rateName);

        string calledName = string.Format(_calledNameFormat, index);
        var called = target.OfType<BooleanValue>().FirstOrDefault(v => v.Name == calledName);

        if (level is null || rate is null || called is null)
            throw new InvalidOperationException($"Missing values for cesspool '{entity.Id}' in target.");

        level.Value = entity.Level;
        rate.Value = entity.Rate;
        called.Value = entity.IsPickupOrdered;
    }
}
