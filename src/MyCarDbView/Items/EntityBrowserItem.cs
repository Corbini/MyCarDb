using CommunityToolkit.Mvvm.ComponentModel;

namespace MyCarDbView.Items;

public sealed partial class EntityBrowserItem : ObservableObject
{
    public required string Title { get; init; }
    public DisplayEntity? Entity { get; init; } = null;
    public required IReadOnlyList<EntityBrowserItem> Items { get; init; }

    [ObservableProperty]
    private bool _isExpanded;

    [ObservableProperty]
    private bool _isPinned;

    public bool IsLeaf => Items.Count == 0;
}
