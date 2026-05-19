namespace MyCarDbWorld.Commands;

public interface ICommandExecutor
{
    event Action? Changed;

    void Execute(IEditCommand command);
}
