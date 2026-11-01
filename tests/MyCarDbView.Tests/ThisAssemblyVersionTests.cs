using System.Reflection;
using System.Text.RegularExpressions;
using MyCarDbView;
using Xunit;

namespace MyCarDbView.Tests.Versioning;

public class ThisAssemblyVersionTests
{
    [Fact]
    public void Version_IsNonEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(ThisAssembly.Version));
    }

    [Fact]
    public void Version_LooksLikeSemver()
    {
        Assert.Matches(
            new Regex(@"^\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?$"),
            ThisAssembly.Version);
    }

    [Fact]
    public void Version_MatchesAssemblyInformationalVersion()
    {
        string info = typeof(ThisAssembly).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

        string semver = info.Split('+')[0];

        Assert.Equal(ThisAssembly.Version, semver);
    }

    [Fact]
    public void Version_MajorMinorPatch_MatchesAssemblyVersion()
    {
        var asmVersion = typeof(ThisAssembly).Assembly.GetName().Version!;
        string prefix = $"{asmVersion.Major}.{asmVersion.Minor}.{asmVersion.Build}";

        Assert.StartsWith(prefix, ThisAssembly.Version);
    }
}
