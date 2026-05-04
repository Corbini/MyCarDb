using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

public interface ILicenseService
{
    IReadOnlyList<LicenseEntry> Licenses { get; }
    LicenseEntry OwnLicense { get; }
}
