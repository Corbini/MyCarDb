using Microsoft.Extensions.DependencyInjection;
using MyCarDbView.Services;
using Xunit;

namespace MyCarDbViewTests.Services;

public class DialogServiceTests
{
    private static IServiceProvider BuildServices() =>
        new ServiceCollection()
            .AddSingleton<IDialogService, DialogService>()
            .BuildServiceProvider();

    private class FooVm : IViewModel { }

    private class ParameterizedVm : IViewModel
    {
        public string Param { get; }
        public ParameterizedVm(string param) => Param = param;
    }

    private class ResultVm : IViewModel, IDialogResult<bool>
    {
        private readonly TaskCompletionSource<bool> _tcs = new();
        public Task<bool> CompletionTask => _tcs.Task;
        public void Complete(bool value) => _tcs.TrySetResult(value);
        public void Cancel() => _tcs.TrySetCanceled();
    }

    private class ParameterizedResultVm : IViewModel, IDialogResult<bool>
    {
        private readonly TaskCompletionSource<bool> _tcs = new();
        public string Param { get; }
        public Task<bool> CompletionTask => _tcs.Task;
        public ParameterizedResultVm(string param) => Param = param;
        public void Complete(bool value) => _tcs.TrySetResult(value);
    }

    private class ThrowingVm : IViewModel
    {
        public ThrowingVm() => throw new InvalidOperationException("Constructor failed");
    }

    [Fact]
    public void CurrentContent_BeforeAnyShow_IsNull()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_RaisesPropertyChanged_ForCurrentContent()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();
        var raised = new List<string?>();
        dialog.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        var task = dialog.ShowAsync<FooVm>();
        await Task.Yield();

        Assert.Contains(nameof(IDialogService.CurrentContent), raised);

        dialog.Close();
        await task;
    }

    [Fact]
    public async Task ShowAsync_WithParametersAndResult_PassesParameterAndReturnsValue()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<ParameterizedResultVm, string, bool>("hello");
        await Task.Yield();

        var vm = (ParameterizedResultVm)dialog.CurrentContent!;
        Assert.Equal("hello", vm.Param);
        vm.Complete(value: true);

        var result = await task;
        Assert.True(result);
        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_Result_Cancellation_ThrowsAndClearsCurrentContent()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();
        using var cts = new CancellationTokenSource();

        var task = dialog.ShowAsync<ResultVm, bool>(cts.Token);
        await Task.Yield();

        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_WhenResultDialogActive_Throws()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var first = dialog.ShowAsync<ResultVm, bool>();
        await Task.Yield();

        await Assert.ThrowsAsync<InvalidOperationException>(() => dialog.ShowAsync<FooVm>());

        var vm = (ResultVm)dialog.CurrentContent!;
        vm.Complete(value: true);
        await first;
    }

    [Fact]
    public void Close_WithoutActiveDialog_DoesNothing()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        dialog.Close();

        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_WhenVmConstructorThrows_StateRemainsClean()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        await Assert.ThrowsAnyAsync<Exception>(() => dialog.ShowAsync<ThrowingVm>());

        Assert.Null(dialog.CurrentContent);

        // verify state is recoverable — next ShowAsync works normally
        var task = dialog.ShowAsync<FooVm>();
        await Task.Yield();
        Assert.IsType<FooVm>(dialog.CurrentContent);

        dialog.Close();
        await task;
    }

    [Fact]
    public async Task ShowAsync_Void_WithPreCancelledToken_Throws()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var task = dialog.ShowAsync<FooVm>(cts.Token);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_Void_SetsCurrentContent()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<FooVm>();
        await Task.Yield();

        Assert.IsType<FooVm>(dialog.CurrentContent);

        dialog.Close();
        await task;
    }

    [Fact]
    public async Task ShowAsync_Void_BlocksUntilClose()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<FooVm>();
        await Task.Yield();

        Assert.False(task.IsCompleted);

        dialog.Close();
        await task.WaitAsync(TimeSpan.FromSeconds(2));

        Assert.True(task.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task ShowAsync_Void_AfterClose_ClearsCurrentContent()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<FooVm>();
        await Task.Yield();
        dialog.Close();
        await task;

        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_Void_Cancellation_ThrowsAndClearsCurrentContent()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();
        using var cts = new CancellationTokenSource();

        var task = dialog.ShowAsync<FooVm>(cts.Token);
        await Task.Yield();

        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_WithParameter_PassesParameterToConstructor()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<ParameterizedVm, string>("hello");
        await Task.Yield();

        var vm = Assert.IsType<ParameterizedVm>(dialog.CurrentContent);
        Assert.Equal("hello", vm.Param);

        dialog.Close();
        await task;
    }

    [Fact]
    public async Task ShowAsync_Result_ReturnsValueFromVm()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<ResultVm, bool>();
        await Task.Yield();

        var vm = (ResultVm)dialog.CurrentContent!;
        vm.Complete(value: true);

        var result = await task;
        Assert.True(result);
    }

    [Fact]
    public async Task ShowAsync_Result_AfterCompletion_ClearsCurrentContent()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<ResultVm, bool>();
        await Task.Yield();

        var vm = (ResultVm)dialog.CurrentContent!;
        vm.Complete(value: false);
        await task;

        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_Result_VmCancellation_ThrowsOperationCanceled()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<ResultVm, bool>();
        await Task.Yield();

        var vm = (ResultVm)dialog.CurrentContent!;
        vm.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
        Assert.Null(dialog.CurrentContent);
    }

    [Fact]
    public async Task ShowAsync_WhenAlreadyOpen_Throws()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var first = dialog.ShowAsync<FooVm>();
        await Task.Yield();

        await Assert.ThrowsAsync<InvalidOperationException>(() => dialog.ShowAsync<FooVm>());

        dialog.Close();
        await first;
    }

    [Fact]
    public async Task Close_DuringResultDialog_DoesNotComplete()
    {
        var sp = BuildServices();
        var dialog = sp.GetRequiredService<IDialogService>();

        var task = dialog.ShowAsync<ResultVm, bool>();
        await Task.Yield();

        dialog.Close();
        await Task.Delay(50);

        Assert.False(task.IsCompleted);
        Assert.NotNull(dialog.CurrentContent);

        var vm = (ResultVm)dialog.CurrentContent!;
        vm.Complete(value: true);
        await task;
    }
}
