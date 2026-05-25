using System;
using System.Collections.Generic;
using MyCarDbView.Items;

namespace MyCarDbView.ViewModels;

public interface IEntityBrowserViewModel
{
    event Action<EntityItem?>? SelectionChanged;

    void UpdateEntities(IEnumerable<EntityItem>? entities);

    void SyncSelection(EntityItem? entity);
}
