using MyCarDbView.Items;

namespace MyCarDbView.ViewModels;

public interface IMapViewModel
{
    event Action<EntityItem?>? SelectionChanged;

    void UpdateEntities(IEnumerable<EntityItem>? entities);

    void SyncSelection(EntityItem? entity);
}
