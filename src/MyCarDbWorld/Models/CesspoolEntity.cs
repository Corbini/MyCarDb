namespace MyCarDbWorld.Models;

public class CesspoolEntity(
    string id,
    float level,
    float rate,
    bool isPickupOrdered) : IEntity
{
    public string Id { get; } = id;
    public float Level { get; internal set; } = level;
    public float Rate { get; internal set; } = rate;
    public bool IsPickupOrdered { get; internal set; } = isPickupOrdered;
}