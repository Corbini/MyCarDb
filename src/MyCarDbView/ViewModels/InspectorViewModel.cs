using System.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbView.Items;
using MyCarDbView.Services;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public sealed partial class InspectorViewModel : ObservableObject, IInspectorViewModel
{
    private readonly IDocumentManager _docMgr;

    public InspectorViewModel(IDocumentManager docMgr)
    {
        _docMgr = docMgr;
    }

    [NotifyPropertyChangedFor(nameof(HasSelection))]
    [ObservableProperty]
    private EntityItem? _selectedEntity;

    partial void OnSelectedEntityChanged(EntityItem? value)
    {
        var session = _docMgr.ActiveDocument
            ?? throw new InvalidOperationException("InspectorViewModel active without document.");

        CurrentInspector = value?.Value switch
        {
            PropertyEntity property => new PropertyEntityInspectorViewModel(session.Stack, property),
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
