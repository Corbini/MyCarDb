using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Items;
using MyCarDbView.Localizations;
using MyCarDbView.Services;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public partial class EntityBrowserViewModel : ObservableObject, IEntityBrowserViewModel
{
    private readonly IPinnedEntitiesService _pinnedService;
    private readonly ILocalizationSource _localization;

    [ObservableProperty]
    private IReadOnlyList<EntityBrowserItem> _categories = [];

    [ObservableProperty]
    private EntityBrowserItem? _selectedNode;
    partial void OnSelectedNodeChanged(EntityBrowserItem? value)
        => SelectionChanged?.Invoke(value?.Entity);

    public event Action<EntityItem?>? SelectionChanged;

    [ObservableProperty]
    private string _searchText = "";
    partial void OnSearchTextChanged(string value) => Rebuild();

    [ObservableProperty]
    private bool _isEmptyResults;

    private List<EntityBrowserItem> _allNodes = [];

    public EntityBrowserViewModel(
        IPinnedEntitiesService pinnedService,
        ILocalizationSource localization)
    {
        _pinnedService = pinnedService;
        _localization = localization;
        _localization.Refreshed += OnLanguageRefreshed;
    }

    public void UpdateEntities(IEnumerable<EntityItem>? entities)
    {
        _allNodes = entities?.Select(entity =>
            new EntityBrowserItem
            {
                Title = null,
                Entity = entity,
                Items = [],
                IsPinned = _pinnedService.Entities.Contains(entity.Value.Id),
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
            _pinnedService.Remove(id);
        else
            _pinnedService.Add(id);

        node.IsPinned = !node.IsPinned;
        Rebuild();
    }

    private void OnLanguageRefreshed() => Rebuild();

    private void Rebuild()
    {
        var pinnedGroup = _allNodes.ToLookup(node => node.IsPinned);
        List<EntityBrowserItem> pinned = [.. pinnedGroup[true]];
        var notPinned = pinnedGroup[false];

        string filter = SearchText.Trim();
        bool hasFilter = !string.IsNullOrWhiteSpace(filter);
        if (hasFilter)
            notPinned = notPinned.Where(n => Match(n, filter));

        Categories = [.. GetPinnedSection(pinned), .. GetCatalogs(notPinned)];
        IsEmptyResults = Categories.Count == 0;
    }

    private static bool Match(EntityBrowserItem item, string filter) =>
        (item.Entity?.Title.Value ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase);

    private IEnumerable<EntityBrowserItem> GetPinnedSection(List<EntityBrowserItem> pinned)
    {
        if (pinned.Count > 0)
            yield return new()
            {
                Title = _localization.Get("EntityBrowser_Section_Pinned"),
                Items = pinned,
                IsExpanded = true,
            };
    }

    private IEnumerable<EntityBrowserItem> GetCatalogs(IEnumerable<EntityBrowserItem> items)
    {
        foreach (var group in items.GroupBy(GetCategoryKey))
        {
            yield return new()
            {
                Title = _localization.Get(group.Key),
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
