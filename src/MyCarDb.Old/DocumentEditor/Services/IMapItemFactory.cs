using DocumentEditor.Items;

namespace DocumentEditor.Services;

public interface IMapItemFactory
{
    MapItem? Create(EntityItem item);
}
