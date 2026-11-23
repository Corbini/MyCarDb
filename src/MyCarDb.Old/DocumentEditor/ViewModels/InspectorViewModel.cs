using System.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using DocumentEditor.Items;
using DocumentEditor.Services;
using GameWorld.Models;

namespace DocumentEditor.ViewModels;

public sealed partial class InspectorViewModel(IDocumentManager docMgr) : ObservableObject, IInspectorViewModel
{
    [NotifyPropertyChangedFor(nameof(HasSelection))]
    [ObservableProperty]
    private EntityItem? _selectedEntity;

    partial void OnSelectedEntityChanged(EntityItem? value)
    {
        var session = docMgr.ActiveDocument
            ?? throw new InvalidOperationException("InspectorViewModel active without document.");

        CurrentInspector = value?.Value switch
        {
            PropertyEntity property => new PropertyEntityInspectorViewModel(session.Stack, property),
            CesspoolEntity cesspool => new CesspoolEntityInspectorViewModel(session.Stack, cesspool),
            WoodOrderEntity woodOrder => new WoodOrderEntityInspectorViewModel(session.Stack, woodOrder),
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
