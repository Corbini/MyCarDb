using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using MyCarDbWorld.Models;

namespace MyCarDbView.Controls;

public partial class EntityBrowser : UserControl
{
    public static readonly StyledProperty<IEnumerable<IEntity>?> EntitiesProperty =
        AvaloniaProperty.Register<EntityBrowser, IEnumerable<IEntity>?>(nameof(Entities));

    public static readonly StyledProperty<IEntity?> SelectedEntityProperty =
        AvaloniaProperty.Register<EntityBrowser, IEntity?>(
            nameof(SelectedEntity),
            defaultBindingMode: BindingMode.TwoWay);

    public EntityBrowserViewModel ViewModel { get; } = new();

    public EntityBrowser()
    {
        InitializeComponent();
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    public IEnumerable<IEntity>? Entities
    {
        get => GetValue(EntitiesProperty);
        set => SetValue(EntitiesProperty, value);
    }

    public IEntity? SelectedEntity
    {
        get => GetValue(SelectedEntityProperty);
        set => SetValue(SelectedEntityProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == EntitiesProperty)
            ViewModel.UpdateEntities(Entities);
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(EntityBrowserViewModel.SelectedNode))
            SelectedEntity = ViewModel.SelectedNode?.Entity;
    }
}
