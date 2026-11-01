using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentEditor.Items;
using DocumentEditor.Services;

namespace DocumentEditor.ViewModels;

public sealed partial class MapViewModel : ObservableObject, IMapViewModel
{
    private readonly IMapItemFactory _factory;

    public MapViewModel(IMapItemFactory factory)
        => _factory = factory;

    [ObservableProperty]
    private IReadOnlyList<MapItem> _items = [];

    [ObservableProperty]
    private MapItem? _selectedItem;

    public event Action<EntityItem?>? SelectionChanged;

    public void UpdateEntities(IEnumerable<EntityItem>? entities)
        => Items = entities?.Select(_factory.Create).OfType<MapItem>().ToList() ?? [];

    public void SyncSelection(EntityItem? entity)
        => SelectedItem = entity is null
            ? null
            : Items.FirstOrDefault(m => ReferenceEquals(m.Value, entity));

    partial void OnSelectedItemChanged(MapItem? oldValue, MapItem? newValue)
    {
        oldValue?.IsSelected = false;
        newValue?.IsSelected = true;

        SelectionChanged?.Invoke(newValue?.Value);
    }

    [RelayCommand]
    private void SelectItem(MapItem? item) => SelectedItem = item;
}
