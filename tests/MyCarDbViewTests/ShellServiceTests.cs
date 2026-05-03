using Microsoft.Extensions.DependencyInjection;
using MyCarDbView.Services;
using Xunit;

namespace MyCarDbViewTests.Services;

public class ShellServiceTests
{
    private static IServiceProvider BuildServices() =>
        new ServiceCollection()
            .AddSingleton<IShellService, ShellService>()
            .BuildServiceProvider();

    private class FooVm : IViewModel { }

    private class ParameterizedVm : IViewModel
    {
        public int Value { get; }
        public ParameterizedVm(int value) => Value = value;
    }

    private class DummyDependency { }

    private class DependentVm : IViewModel
    {
        public DummyDependency Dep { get; }
        public DependentVm(DummyDependency dep) => Dep = dep;
    }

    [Fact]
    public void CurrentContent_BeforeAnyNavigation_IsNull()
    {
        var sp = BuildServices();
        var shell = sp.GetRequiredService<IShellService>();

        Assert.Null(shell.CurrentContent);
    }

    [Fact]
    public void NavigateTo_SetsCurrentContent_ToInstanceOfRequestedType()
    {
        var sp = BuildServices();
        var shell = sp.GetRequiredService<IShellService>();

        shell.NavigateTo<FooVm>();

        Assert.IsType<FooVm>(shell.CurrentContent);
    }

    [Fact]
    public void NavigateTo_RaisesPropertyChanged_ForCurrentContent()
    {
        var sp = BuildServices();
        var shell = sp.GetRequiredService<IShellService>();
        var raised = new List<string?>();
        shell.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        shell.NavigateTo<FooVm>();

        Assert.Contains(nameof(IShellService.CurrentContent), raised);
    }

    [Fact]
    public void NavigateTo_WithParameter_PassesParameterToConstructor()
    {
        var sp = BuildServices();
        var shell = sp.GetRequiredService<IShellService>();

        shell.NavigateTo<ParameterizedVm, int>(42);

        var vm = Assert.IsType<ParameterizedVm>(shell.CurrentContent);
        Assert.Equal(42, vm.Value);
    }

    [Fact]
    public void NavigateTo_Twice_ReplacesCurrentContent()
    {
        var sp = BuildServices();
        var shell = sp.GetRequiredService<IShellService>();

        shell.NavigateTo<FooVm>();
        var first = shell.CurrentContent;
        shell.NavigateTo<FooVm>();
        var second = shell.CurrentContent;

        Assert.NotSame(first, second);
    }

    [Fact]
    public void NavigateTo_ResolvesDependenciesViaDI()
    {
        var sp = new ServiceCollection()
            .AddSingleton<IShellService, ShellService>()
            .AddSingleton<DummyDependency>()
            .BuildServiceProvider();
        var shell = sp.GetRequiredService<IShellService>();

        shell.NavigateTo<DependentVm>();

        var vm = Assert.IsType<DependentVm>(shell.CurrentContent);
        Assert.NotNull(vm.Dep);
    }
}
