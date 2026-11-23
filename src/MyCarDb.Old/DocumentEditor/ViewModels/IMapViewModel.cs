using DocumentEditor.Items;

namespace DocumentEditor.ViewModels;

public interface IMapViewModel
{
    event Action<EntityItem?>? SelectionChanged;

    void UpdateEntities(IEnumerable<EntityItem>? entities);

    void SyncSelection(EntityItem? entity);
}
