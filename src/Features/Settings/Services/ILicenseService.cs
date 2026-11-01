using Settings.Models.AppState;

namespace Settings.Services;

public interface ILicenseService
{
    IReadOnlyList<LicenseEntry> Licenses { get; }
    LicenseEntry OwnLicense { get; }
}
