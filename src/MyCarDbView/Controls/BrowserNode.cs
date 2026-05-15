using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbWorld.Models;

namespace MyCarDbView.Controls;

public sealed partial class BrowserNode : ObservableObject
{
    public required string Title { get; init; }
    public required IEntity? Entity { get; init; }
    public required IReadOnlyList<BrowserNode> Items { get; init; }

    [ObservableProperty]
    private bool _isExpanded;

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private bool _isPinned;

    public bool IsLeaf => Entity is not null;

}
