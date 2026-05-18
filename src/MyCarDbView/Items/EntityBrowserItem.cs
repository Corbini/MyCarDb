using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbView.Localizations;

namespace MyCarDbView.Items;

public sealed partial class EntityBrowserItem : ObservableObject
{
    public required LocalizedString? Title { get; init; }
    public EntityItem? Entity { get; init; } = null;
    public required IReadOnlyList<EntityBrowserItem> Items { get; init; }

    [ObservableProperty]
    private bool _isExpanded;

    [ObservableProperty]
    private bool _isPinned;

    public bool IsLeaf => Items.Count == 0;
}
