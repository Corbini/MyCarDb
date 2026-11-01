using Contracts;

namespace MyCarDbView.Services;

internal sealed class ApplicationInfo : IApplicationInfo
{
    public string Version => ThisAssembly.Version;
}
