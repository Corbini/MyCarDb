using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Items;
using MyCarDbView.Services;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public partial class EntityBrowserViewModel(IPinnedEntitiesService pinnedService) : ObservableObject
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

    public void UpdateEntities(IEnumerable<DisplayEntity>? entities)
    {
        _allNodes = entities?.Select(entity =>
            new EntityBrowserItem
            {
                Title = entity.TitleKey,
                Entity = entity,
                Items = [],
                IsPinned = pinnedService.Entities.Contains(entity.Value.Id),
            }).ToList()
            ?? [];

        Rebuild();
    }

    [RelayCommand]
    private void TogglePin(EntityBrowserItem node)
    {
        if (node.Entity?.Value.Id is not string id)
            return;

        if (node.IsPinned)
            pinnedService.Remove(id);
        else
            pinnedService.Add(id);

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
                Title = "EntityBrowser_Section_Pinned",
                Items = pinned,
                IsExpanded = true,
            };
    }

    private static IEnumerable<EntityBrowserItem> GetCatalogs(IEnumerable<EntityBrowserItem> items)
    {
        foreach (var group in items.GroupBy(GetCategoryKey))
        {
            yield return new()
            {
                Title = group.Key,
                Items = [.. group],
                IsExpanded = true,
            };
        }
    }

    private static string GetCategoryKey(EntityBrowserItem item) => item.Entity?.Value switch
    {
        PropertyEntity => "Entity_Category_Items",
        _ => "Entity_Category_Others",
    };
}
