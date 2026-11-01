using Contracts;
using MyCarDbView.Localizations;
using Xunit;

namespace MyCarDbViewTests.Localizations;

public class LocalizationSourceTests
{
    [Fact]
    public void Get_SameKey_ReturnsSameInstance()
    {
        var first = LocalizationSource.Instance.Get("_TEST_LSrc_SameKey");
        var second = LocalizationSource.Instance.Get("_TEST_LSrc_SameKey");

        Assert.Same(first, second);
    }

    [Fact]
    public void Get_DifferentKeys_ReturnDifferentInstances()
    {
        var a = LocalizationSource.Instance.Get("_TEST_LSrc_DiffKey_A");
        var b = LocalizationSource.Instance.Get("_TEST_LSrc_DiffKey_B");

        Assert.NotSame(a, b);
    }

    [Fact]
    public void Refresh_FiresRefreshedEvent()
    {
        bool raised = false;
        void Handler() => raised = true;
        LocalizationSource.Instance.Refreshed += Handler;

        try
        {
            LocalizationSource.Instance.Refresh();
        }
        finally
        {
            LocalizationSource.Instance.Refreshed -= Handler;
        }

        Assert.True(raised);
    }

    [Fact]
    public void LocalizedString_ValueChange_FiresPropertyChanged()
    {
        var ls = new LocalizedString("k");
        bool raised = false;
        ls.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(LocalizedString.Value))
                raised = true;
        };

        ls.Value = "new";

        Assert.True(raised);
    }
}
