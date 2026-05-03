using Avalonia.Controls.Templates;
using MyCarDbView.MVVM;
using MyCarDbView.Services;
using Xunit;

namespace MyCarDbViewTests.MVVM;

public class ViewLocatorTests
{
    private class UnknownVm : IViewModel { }

    [Fact]
    public void Build_UnknownViewModel_ReturnsNull()
    {
        var locator = new ViewLocator();
        var vm = new UnknownVm();

        var view = locator.Build(vm);

        Assert.Null(view);
    }

    [Fact]
    public void Match_ViewModelInstance_ReturnsTrue()
    {
        var locator = (IDataTemplate)new ViewLocator();
        var vm = new UnknownVm();

        Assert.True(locator.Match(vm));
    }

    [Fact]
    public void Match_NonViewModel_ReturnsFalse()
    {
        var locator = (IDataTemplate)new ViewLocator();

        Assert.False(locator.Match("not a vm"));
        Assert.False(locator.Match(null));
        Assert.False(locator.Match(42));
    }
}
