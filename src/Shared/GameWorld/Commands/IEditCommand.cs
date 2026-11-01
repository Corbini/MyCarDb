using GameWorld.Models;

namespace GameWorld.Commands;

public interface IEditCommand
{
    string DisplayNameKey { get; }

    void Apply(World world);

    void Revert(World world);
}
