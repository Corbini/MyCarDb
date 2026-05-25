using MyCarDbWorld;
using MyCarDbWorld.Coders;
using MyCarDbWorld.Commands;
using MyCarDbWorld.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMyCarDbWorld(this IServiceCollection services)
    {
        services.AddSingleton<IWorldCoder, WorldCoder>();
        services.AddSingleton<IEntityCoder<PropertyEntity>, PropertyCoder>();
        services.AddSingleton<IEntityCoder<CesspoolEntity>, CesspoolCoder>();
        return services;
    }
}
