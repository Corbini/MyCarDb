using System.Xml.Linq;
using Xunit;

namespace MyCarDbViewTests.Localizations;

public class ResxParityTests
{
    private static string LocalizationsDir
    {
        get
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "MyCarDb.slnx")))
                dir = dir.Parent;
            if (dir is null)
                throw new InvalidOperationException("Could not locate solution root (MyCarDb.slnx).");
            return Path.Combine(dir.FullName, "src", "MyCarDbView", "Localizations");
        }
    }

    private static HashSet<string> ReadKeys(string resxPath)
    {
        var doc = XDocument.Load(resxPath);
        return doc.Root!.Elements("data")
            .Select(e => e.Attribute("name")!.Value)
            .ToHashSet();
    }

    [Theory]
    [InlineData("ViewLocalization")]
    [InlineData("DbLocalization")]
    public void DefaultAndPolishResx_HaveIdenticalKeySet(string baseName)
    {
        string defaultPath = Path.Combine(LocalizationsDir, $"{baseName}.resx");
        string plPath = Path.Combine(LocalizationsDir, $"{baseName}.pl.resx");

        var defaultKeys = ReadKeys(defaultPath);
        var plKeys = ReadKeys(plPath);

        var missingInPolish = defaultKeys.Except(plKeys).OrderBy(k => k).ToList();
        var extraInPolish = plKeys.Except(defaultKeys).OrderBy(k => k).ToList();

        Assert.True(
            missingInPolish.Count == 0 && extraInPolish.Count == 0,
            $"{baseName}: key sets differ.\n" +
            $"  Missing in {baseName}.pl.resx: {(missingInPolish.Count == 0 ? "(none)" : string.Join(", ", missingInPolish))}\n" +
            $"  Extra in {baseName}.pl.resx:   {(extraInPolish.Count == 0 ? "(none)" : string.Join(", ", extraInPolish))}");
    }
}
