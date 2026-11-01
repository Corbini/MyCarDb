using MyCarDb;
using StylishDesign;

namespace Microsoft.Extensions.DependencyInjection;

public static class MyCarDbDependencyInjection
{
    public static IServiceCollection AddMyCarDb(this IServiceCollection services)
    {
        services.AddStylishDesign();
        services.AddScoped<IThemePersistence, LocalStorageThemePersistence>();
        return services;
    }
}
