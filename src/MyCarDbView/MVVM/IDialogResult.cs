namespace MyCarDbView.Services;

public interface IDialogResult
{
    Task CompletionTask { get; }
}

public interface IDialogResult<TResult>
{
    Task<TResult> CompletionTask { get; }
}
