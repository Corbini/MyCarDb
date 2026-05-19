using MyCarDbWorld.Models;

namespace MyCarDbWorld.Commands;

public interface IEditCommand
{
    string DisplayNameKey { get; }

    void Apply(World world);

    void Revert(World world);
}
