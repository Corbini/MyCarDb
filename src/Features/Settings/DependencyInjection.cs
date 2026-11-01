using Settings.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class SettingsDependencyInjection
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddSingleton<ILanguageService, LanguageService>();
        return services;
    }
}
