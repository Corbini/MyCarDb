using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Items;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public partial class EntityBrowserViewModel : ObservableObject
{
    [ObservableProperty]
    private IReadOnlyList<EntityBrowserItem> _categories = [];

    [ObservableProperty]
    private EntityBrowserItem? _selectedNode;

    [ObservableProperty]
    private string _searchText = "";
    partial void OnSearchTextChanged(string value) => Rebuild();

    [ObservableProperty]
    private bool _isEmptyResults;

    private List<EntityBrowserItem> _allNodes = [];
    private readonly HashSet<string> _pinnedIds = [];

    public void UpdateEntities(IEnumerable<IEntity>? entities)
    {
        _allNodes = entities?.Select(entity =>
            new EntityBrowserItem
            {
                Title = entity.Id,
                Entity = entity,
                Items = [],
                IsPinned = _pinnedIds.Contains(entity.Id),
            }).ToList()
            ?? [];

        Rebuild();
    }

    [RelayCommand]
    private void TogglePin(EntityBrowserItem node)
    {
        if (node.Entity?.Id is not string id)
            return;

        if (!_pinnedIds.Add(id))
            _pinnedIds.Remove(id);

        node.IsPinned = !node.IsPinned;
        Rebuild();
    }

    private void Rebuild()
    {
        var pinnedGroup = _allNodes.ToLookup(node => node.IsPinned);
        List<EntityBrowserItem> pinned = [.. pinnedGroup[true]];
        var notPinned = pinnedGroup[false];

        string filter = SearchText.Trim();
        bool hasFilter = !string.IsNullOrWhiteSpace(filter);
        if (hasFilter)
            notPinned = notPinned.Where(n => n.Title.Contains(filter, StringComparison.OrdinalIgnoreCase));

        Categories = [.. GetPinnedSection(pinned), .. GetCatalogs(notPinned)];
        IsEmptyResults = Categories.Count == 0;
    }

    private static IEnumerable<EntityBrowserItem> GetPinnedSection(List<EntityBrowserItem> pinned)
    {
        if (pinned.Count > 0)
            yield return new()
            {
                Title = "EntityBrowser_pinned",
                Items = pinned,
                IsExpanded = true,
            };
    }

    private IEnumerable<EntityBrowserItem> GetCatalogs(IEnumerable<EntityBrowserItem> items)
    {
        foreach (var group in items.GroupBy(item => item.Entity?.GetType().Name ?? "unknown_type"))
        {
            yield return new()
            {
                Title = group.Key,
                Items = [.. group],
                IsExpanded = true,
            };
        }
    }
}
