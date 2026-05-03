using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace MyCarDbView.Services;

internal sealed partial class DialogService(IServiceProvider services) : ObservableObject, IDialogService
{
    [ObservableProperty]
    private IViewModel? _currentContent;

    // TCS dla void variant — pozwala ShowAsync<TVM>() awaitować do momentu Close().
    private TaskCompletionSource? _voidTcs;

    public async Task ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel
    {
        EnsureNoDialog();
        var vm = ActivatorUtilities.CreateInstance<TViewModel>(services);
        await RunVoidAsync(vm, cancellationToken);
    }

    public async Task ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameters>(
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel
        where TParameters : notnull
    {
        EnsureNoDialog();
        var vm = ActivatorUtilities.CreateInstance<TViewModel>(services, parameters);
        await RunVoidAsync(vm, cancellationToken);
    }

    public async Task<TResult> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TResult>(
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel, IDialogResult<TResult>
    {
        EnsureNoDialog();
        var vm = ActivatorUtilities.CreateInstance<TViewModel>(services);
        return await RunResultAsync(vm, cancellationToken);
    }

    public async Task<TResult> ShowAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameters, TResult>(
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TViewModel : class, IViewModel, IDialogResult<TResult>
        where TParameters : notnull
    {
        EnsureNoDialog();
        var vm = ActivatorUtilities.CreateInstance<TViewModel>(services, parameters);
        return await RunResultAsync(vm, cancellationToken);
    }

    public void Close() => _voidTcs?.TrySetResult();

    private async Task RunVoidAsync(IViewModel vm, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        _voidTcs = tcs;
        await using var _ = ct.Register(() => tcs.TrySetCanceled(ct)).ConfigureAwait(false);
        try
        {
            CurrentContent = vm;
            await tcs.Task.ConfigureAwait(true);
        }
        finally
        {
            if (ReferenceEquals(CurrentContent, vm))
                CurrentContent = null;
            if (ReferenceEquals(_voidTcs, tcs))
                _voidTcs = null;
        }
    }

    private async Task<TResult> RunResultAsync<TResult>(IDialogResult<TResult> vm, CancellationToken ct)
    {
        try
        {
            CurrentContent = (IViewModel)vm;
            return await vm.CompletionTask.WaitAsync(ct).ConfigureAwait(true);
        }
        finally
        {
            if (ReferenceEquals(CurrentContent, vm))
                CurrentContent = null;
        }
    }

    private void EnsureNoDialog()
    {
        if (CurrentContent is not null)
            throw new InvalidOperationException(
                "A dialog is already open. Close it before showing another.");
    }
}
