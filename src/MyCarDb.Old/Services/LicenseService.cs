using Avalonia.Platform;
using Settings.Models.AppState;
using Settings.Services;

namespace MyCarDbView.Services;

internal sealed class LicenseService : ILicenseService
{
    public IReadOnlyList<LicenseEntry> Licenses { get; }
    public LicenseEntry OwnLicense { get; }

    public LicenseService()
    {
        Licenses = _manifest
            .Select(m => new LicenseEntry(m.Component, m.LicenseType, LoadExternalLicenses(m.FileName)))
            .ToList();

        OwnLicense = new("MyCarDbView", "MIT", LoadOwnLicense());
    }

    private static readonly (string Component, string LicenseType, string FileName)[] _manifest =
    [
        ("Phosphor Icons",            "MIT",         "phosphor-icons.LICENSE"),
        ("Inter (font)",              "SIL OFL 1.1", "inter.LICENSE"),
        ("Avalonia UI",               "MIT",         "avalonia.LICENSE"),
        (".NET Community Toolkit",    "MIT",         "communitytoolkit.LICENSE"),
        (".NET Foundation libraries", "MIT",         "dotnet-foundation.LICENSE"),
    ];


    private static string LoadOwnLicense()
    {
        var uri = new Uri("avares://MyCarDbView/Assets/LICENSE");
        using var stream = AssetLoader.Open(uri);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static string LoadExternalLicenses(string fileName)
    {
        var uri = new Uri($"avares://MyCarDbView/Assets/Licenses/{fileName}");
        using var stream = AssetLoader.Open(uri);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
