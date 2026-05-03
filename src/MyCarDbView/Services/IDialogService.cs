using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace MyCarDbView.Services;

public interface IDialogService : INotifyPropertyChanged
{
    IViewModel? CurrentContent { get; }

    Task ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel;

    Task ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameters>(
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel
        where TParameters : notnull;

    Task<TResult> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TResult>(
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel, IDialogResult<TResult>;

    Task<TResult> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameters, TResult>(
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel, IDialogResult<TResult>
        where TParameters : notnull;

    // Programowe zamknięcie aktywnego dialogu void-variant.
    // No-op gdy aktywny dialog ma IDialogResult — wtedy VM kontroluje CompletionTask.
    void Close();
}
