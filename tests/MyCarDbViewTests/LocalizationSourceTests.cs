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
    public void Refresh_RaisesPropertyChangedForValueOnCachedInstance()
    {
        var ls = LocalizationSource.Instance.Get("_TEST_LSrc_RefreshKey");
        var raised = new List<string?>();
        ls.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        LocalizationSource.Instance.Refresh();

        Assert.Contains(nameof(LocalizedString.Value), raised);
    }
}
