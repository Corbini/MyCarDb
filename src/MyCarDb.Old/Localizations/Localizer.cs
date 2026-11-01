using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace MyCarDbView.Localizations;

public static class Localizer
{
    private static readonly ResourceManagerStringLocalizerFactory _factory = new(
        Options.Create(new LocalizationOptions { ResourcesPath = string.Empty }),
        NullLoggerFactory.Instance);

    public static IStringLocalizer<ViewLocalization> View { get; } =
        new StringLocalizer<ViewLocalization>(_factory);

    public static IStringLocalizer<DbLocalization> Db { get; } =
        new StringLocalizer<DbLocalization>(_factory);
}
