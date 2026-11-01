using System;
using System.Collections.Generic;
using DocumentEditor.Items;

namespace DocumentEditor.ViewModels;

public interface IEntityBrowserViewModel
{
    event Action<EntityItem?>? SelectionChanged;

    void UpdateEntities(IEnumerable<EntityItem>? entities);

    void SyncSelection(EntityItem? entity);
}
