using MyCarDbWorld.Models;

namespace MyCarDbWorld.Commands;

public sealed class CommandStack(World world) : ICommandStack, ICommandExecutor
{
    private readonly Stack<IEditCommand> _undo = new();
    private readonly Stack<IEditCommand> _redo = new();
    private World _world = world;

    public event Action? Changed;

    public bool CanUndo => _undo.Count > 0;

    public bool CanRedo => _redo.Count > 0;

    public void Execute(IEditCommand command)
    {
        command.Apply(_world);
        _undo.Push(command);
        _redo.Clear();
        Changed?.Invoke();
    }

    public void Undo()
    {
        if (!CanUndo)
            return;

        var command = _undo.Pop();
        command.Revert(_world);
        _redo.Push(command);
        Changed?.Invoke();
    }

    public void Redo()
    {
        if (!CanRedo)
            return;

        var command = _redo.Pop();
        command.Apply(_world);
        _undo.Push(command);
        Changed?.Invoke();
    }

    public void Clear()
    {
        if (_undo.Count == 0 && _redo.Count == 0)
            return;

        _undo.Clear();
        _redo.Clear();
        Changed?.Invoke();
    }

    public void ChangeWorld(World world)
    {
        _world = world;
        if (_undo.Count == 0 && _redo.Count == 0)
            return;

        _undo.Clear();
        _redo.Clear();
        Changed?.Invoke();
    }
}
