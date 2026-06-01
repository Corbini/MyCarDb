using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbWorld.Commands;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public sealed partial class WoodOrderEntityInspectorViewModel : ObservableObject, IDisposable
{
    private readonly WoodOrderEntity _entity;
    private readonly ICommandExecutor _stack;
    private bool _isSyncing;

    [ObservableProperty]
    private bool _isOrdered;

    [ObservableProperty]
    private decimal _wait;

    public WoodOrderEntityInspectorViewModel(
        ICommandExecutor stack,
        WoodOrderEntity entity)
    {
        _stack = stack;
        _entity = entity;
        _isSyncing = true;
        IsOrdered = entity.IsOrdered;
        Wait = (decimal)entity.Wait;
        _isSyncing = false;
        _stack.Changed += SyncFromEntity;
    }

    partial void OnIsOrderedChanged(bool value)
    {
        if (_isSyncing) return;
        if (value == _entity.IsOrdered) return;
        _stack.Execute(new SetWoodOrderCommand(_entity.Id, _entity.IsOrdered, value));
    }

    partial void OnWaitChanged(decimal value)
    {
        if (_isSyncing) return;
        var newWait = (float)value;
        if (newWait == _entity.Wait) return;
        _stack.Execute(new SetWoodOrderWaitCommand(_entity.Id, _entity.Wait, newWait));
    }

    private void SyncFromEntity()
    {
        _isSyncing = true;
        IsOrdered = _entity.IsOrdered;
        Wait = (decimal)_entity.Wait;
        _isSyncing = false;
    }

    public void Dispose() => _stack.Changed -= SyncFromEntity;
}
