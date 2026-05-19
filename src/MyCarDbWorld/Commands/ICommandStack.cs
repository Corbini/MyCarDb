using MyCarDbWorld.Models;

namespace MyCarDbWorld.Commands;

public interface ICommandStack
{
    event Action? Changed;

    bool CanUndo { get; }

    bool CanRedo { get; }

    void Undo();

    void Redo();

    void Clear();

    void ChangeWorld(World world);
}
