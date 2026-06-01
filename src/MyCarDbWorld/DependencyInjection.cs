using MyCarDbWorld;
using MyCarDbWorld.Coders;
using MyCarDbWorld.Commands;
using MyCarDbWorld.Data;
using MyCarDbWorld.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMyCarDbWorld(this IServiceCollection services)
    {
        services.AddSingleton<IMapDataRegistry, MapDataRegistry>();
        services.AddSingleton<IWorldCoder, WorldCoder>();
        services.AddSingleton<IEntityCoder<PropertyEntity>, PropertyCoder>();
        services.AddSingleton<IEntityCoder<CesspoolEntity>, CesspoolCoder>();
        services.AddSingleton<IEntityCoder<WoodOrderEntity>, WoodOrderCoder>();
        return services;
    }
}
