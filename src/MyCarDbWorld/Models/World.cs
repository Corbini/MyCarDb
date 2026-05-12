using MyCarDb.Values;

namespace MyCarDbWorld.Models;

public sealed record World
{
    public List<IEntity> Entities { get; set; } = [];

    public List<IFileValue> Values { get; set; } = [];
}
