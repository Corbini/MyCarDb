using GameWorld;
using GameWorld.Coders;
using GameWorld.Commands;
using GameWorld.Data;
using GameWorld.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddGameWorld(this IServiceCollection services)
    {
        services.AddSingleton<IMapDataRegistry, MapDataRegistry>();
        services.AddSingleton<IWorldCoder, WorldCoder>();
        services.AddSingleton<IEntityCoder<PropertyEntity>, PropertyCoder>();
        services.AddSingleton<IEntityCoder<CesspoolEntity>, CesspoolCoder>();
        services.AddSingleton<IEntityCoder<WoodOrderEntity>, WoodOrderCoder>();
        return services;
    }
}
