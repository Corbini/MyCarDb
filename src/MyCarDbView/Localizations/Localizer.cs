using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace MyCarDbView.Localizations;

public static class Localizer
{
    private static readonly ResourceManagerStringLocalizerFactory Factory = new(
        Options.Create(new LocalizationOptions { ResourcesPath = string.Empty }),
        NullLoggerFactory.Instance);

    public static IStringLocalizer<ViewLocalization> View { get; } =
        new StringLocalizer<ViewLocalization>(Factory);

    public static IStringLocalizer<DbLocalization> Db { get; } =
        new StringLocalizer<DbLocalization>(Factory);
}
