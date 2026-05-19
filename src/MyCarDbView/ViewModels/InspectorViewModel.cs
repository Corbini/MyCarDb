using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbView.Items;
using MyCarDbWorld.Commands;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public sealed partial class InspectorViewModel : ObservableObject
{
    private readonly ICommandExecutor _stack;

    public InspectorViewModel(ICommandExecutor stack)
    {
        _stack = stack;
    }

    [NotifyPropertyChangedFor(nameof(HasSelection))]
    [ObservableProperty]
    private EntityItem? _selectedEntity;

    partial void OnSelectedEntityChanged(EntityItem? value)
    {
        CurrentInspector = value?.Value switch
        {
            PropertyEntity property => new PropertyEntityInspectorViewModel(_stack, property),
            _ => null,
        };
    }

    [ObservableProperty]
    private object? _currentInspector;

    private object? _previousInspector;

    partial void OnCurrentInspectorChanged(object? value)
    {
        (_previousInspector as IDisposable)?.Dispose();
        _previousInspector = value;
    }

    public bool HasSelection => SelectedEntity is not null;
}
