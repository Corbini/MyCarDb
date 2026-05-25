using MyCarDbView.Items;

namespace MyCarDbView.Services;

public interface IMapItemFactory
{
    MapItem? Create(EntityItem item);
}
