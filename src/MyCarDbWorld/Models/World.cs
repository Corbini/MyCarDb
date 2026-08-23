using MyCarDb.Values;

namespace MyCarDbWorld.Models;

public sealed record World
{
    internal List<IEntity> MutableEntities { get; init; } = [];

    internal List<IFileValue> MutableValues { get; init; } = [];

    public IReadOnlyList<IEntity> Entities => MutableEntities;

    public IReadOnlyList<IFileValue> Values => MutableValues;
}
