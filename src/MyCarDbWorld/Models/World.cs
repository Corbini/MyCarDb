using MyCarDb.Values;

namespace MyCarDbWorld.Models;

public sealed record World
{
    public List<IFileValue> Values { get; set; } = [];
}
