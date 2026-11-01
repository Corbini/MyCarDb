using System.Numerics;
using GameWorld.Models;
using SaveFile.Values;

namespace GameWorld.Coders;

public sealed class PropertyCoder : IEntityCoder<PropertyEntity>
{
    private readonly string[] _properties = [
      "floor jack(itemx)",
      "car jack(itemx)",
      "spanner set(itemx)",
      "water bucket(itemx)",
      "wiring mess(itemx)",
      "warningtrianglex(item1)",
      "fish trap(itemx)",
      "parts magazine(itemx)",
      "diggingbar(item1)",
      "sledgehammer(item1)",
      "ax(item3)",
      "ax(item4)",
      "dipper(item3)",
      "dipper(item4)",
      "sandbag(item1)",
      "sandbag(item2)",
      "sandbag(item3)",
      "garbage barrel(itemx)",
      "gasoline(itemx)",
      "diesel(itemx)",
      "notepad(itemx)",
      "lantern(itemx)",
      "camera(item1)",
      "sofa(itemx)",
      "tv remote control(item1)",
      "tv remote control(item2)",
      "fireworks bag(itemx)",
      "fireworksbag0",
      "basketball(Clone)",
      "ratchet set(Clone)",
      "WoodCutter"
    ];

    public IEnumerable<PropertyEntity> Decode(IReadOnlyList<IFileValue> source)
    {
        foreach (string property in _properties)
        {
            var pos = source.OfType<PositionValue>().FirstOrDefault(v => v.Name == property);
            if (pos == null)
                continue;

            yield return new PropertyEntity(pos.Name, new Vector2(pos.AxisX, pos.AxisZ));
        }
    }

    public void Encode(PropertyEntity entity, IReadOnlyList<IFileValue> target)
    {
        var pos = target.OfType<PositionValue>().FirstOrDefault(v => v.Name == entity.Id);
        if (pos is null)
            throw new InvalidOperationException($"No PositionValue named '{entity.Id}' in target.");

        pos.AxisX = entity.Position.X;
        pos.AxisZ = entity.Position.Y;
    }
}
