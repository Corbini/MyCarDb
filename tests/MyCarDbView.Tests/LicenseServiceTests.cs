using MyCarDbView.Services;
using Xunit;

namespace MyCarDbView.Tests.Services;

[Collection(nameof(AvaloniaCollection))]
public class LicenseServiceTests
{
    private static readonly string[] _expectedComponents =
    [
        "Phosphor Icons",
        "Inter (font)",
        "Avalonia UI",
        ".NET Community Toolkit",
        ".NET Foundation libraries",
    ];

    [Fact]
    public void Licenses_HasEntryForEveryBundledThirdParty()
    {
        var sut = new LicenseService();

        Assert.Equal(_expectedComponents.Length, sut.Licenses.Count);
        Assert.Equal(_expectedComponents, sut.Licenses.Select(l => l.Component));
    }

    [Fact]
    public void Licenses_EveryEntryHasLicenseTypeAndNonEmptyContent()
    {
        var sut = new LicenseService();

        Assert.All(sut.Licenses, entry =>
        {
            Assert.False(string.IsNullOrWhiteSpace(entry.Component));
            Assert.False(string.IsNullOrWhiteSpace(entry.LicenseType));
            Assert.False(string.IsNullOrWhiteSpace(entry.Content));
        });
    }

    [Fact]
    public void OwnLicense_IsMyCarDbViewUnderMit()
    {
        var sut = new LicenseService();

        Assert.Equal("MyCarDbView", sut.OwnLicense.Component);
        Assert.Equal("MIT", sut.OwnLicense.LicenseType);
    }

    [Fact]
    public void OwnLicense_ContentMatchesBundledLicenseFile()
    {
        var sut = new LicenseService();

        Assert.Contains("MIT License", sut.OwnLicense.Content);
        Assert.Contains("Patryk Strama", sut.OwnLicense.Content);
    }

    [Fact]
    public void PhosphorIcons_LicenseIsLoadedAsMit()
    {
        var sut = new LicenseService();

        var phosphor = sut.Licenses.Single(l => l.Component == "Phosphor Icons");
        Assert.Equal("MIT", phosphor.LicenseType);
        Assert.False(string.IsNullOrWhiteSpace(phosphor.Content));
    }

    [Fact]
    public void InterFont_LicenseIsLoadedAsSilOfl()
    {
        var sut = new LicenseService();

        var inter = sut.Licenses.Single(l => l.Component == "Inter (font)");
        Assert.Equal("SIL OFL 1.1", inter.LicenseType);
        Assert.False(string.IsNullOrWhiteSpace(inter.Content));
    }

    [Fact]
    public void Licenses_DistinctInstancesPerCall_ButSameContent()
    {
        var first = new LicenseService();
        var second = new LicenseService();

        Assert.NotSame(first.Licenses, second.Licenses);
        Assert.Equal(
            first.Licenses.Select(l => (l.Component, l.LicenseType, l.Content)),
            second.Licenses.Select(l => (l.Component, l.LicenseType, l.Content)));
    }
}
