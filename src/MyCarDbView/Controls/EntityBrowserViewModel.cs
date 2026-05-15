using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCarDbWorld.Models;

namespace MyCarDbView.Controls;

public partial class EntityBrowserViewModel : ObservableObject
{
    private const string _categoryTitle = "EntityBrowser_Category_Items";

    [ObservableProperty]
    private IReadOnlyList<BrowserNode> _categories = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPinnedEmpty))]
    private IReadOnlyList<BrowserNode> _pinned = [];

    [ObservableProperty]
    private BrowserNode? _selectedNode;

    [ObservableProperty]
    private string _searchText = "";

    [ObservableProperty]
    private bool _isEmptyResults;

    public bool IsPinnedEmpty => Pinned.Count == 0;
    private List<BrowserNode> _allNodes = [];

    public void UpdateEntities(IEnumerable<IEntity>? entities)
    {
        _allNodes = entities is null
            ? []
            : entities.OfType<PropertyEntity>()
                .Select(e => new BrowserNode { Title = e.Id, Entity = e, Items = [] })
                .ToList();

        SyncSelectionToNodes();
        Rebuild();
    }

    [RelayCommand]
    private void SelectNode(BrowserNode? node) => SelectedNode = node;

    partial void OnSelectedNodeChanged(BrowserNode? value) => SyncSelectionToNodes();

    partial void OnSearchTextChanged(string value) => Rebuild();

    [RelayCommand]
    private void TogglePin(BrowserNode? node)
    {
        if (node is null || !node.IsLeaf)
            return;

        node.IsPinned = !node.IsPinned;
        Rebuild();
    }

    private void SyncSelectionToNodes()
    {
        foreach (BrowserNode n in _allNodes)
            n.IsSelected = ReferenceEquals(n, SelectedNode);
    }

    private void Rebuild()
    {
        string filter = SearchText.Trim();
        bool hasFilter = filter.Length > 0;

        Pinned = _allNodes.Where(n => n.IsPinned).ToList();

        IEnumerable<BrowserNode> notPinned = _allNodes.Where(n => !n.IsPinned);
        List<BrowserNode> matching = hasFilter
            ? notPinned.Where(n => n.Title.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList()
            : notPinned.ToList();

        if (hasFilter && matching.Count == 0)
        {
            Categories = [];
            IsEmptyResults = true;
            return;
        }

        Categories = matching.Count == 0
            ? []
            :
            [
                new BrowserNode
                {
                    Title = _categoryTitle,
                    Entity = null,
                    Items = matching,
                    IsExpanded = true,
                },
            ];
        IsEmptyResults = false;
    }
}
