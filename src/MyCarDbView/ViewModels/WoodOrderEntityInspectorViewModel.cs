using CommunityToolkit.Mvvm.ComponentModel;
using GameWorld.Commands;
using GameWorld.Models;

namespace MyCarDbView.ViewModels;

public sealed partial class WoodOrderEntityInspectorViewModel : ObservableObject, IDisposable
{
    private readonly WoodOrderEntity _entity;
    private readonly ICommandExecutor _stack;

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
        IsOrdered = entity.IsOrdered;
        Wait = (decimal)entity.Wait;
        _stack.Changed += SyncFromEntity;
    }

    partial void OnIsOrderedChanged(bool value)
    {
        if (value == _entity.IsOrdered)
            return;
        _stack.Execute(new SetWoodOrderCommand(_entity.Id, _entity.IsOrdered, value));
    }

    partial void OnWaitChanged(decimal value)
    {
        var newWait = (float)value;
        if (newWait == _entity.Wait)
            return;
        _stack.Execute(new SetWoodOrderWaitCommand(_entity.Id, _entity.Wait, newWait));
    }

    private void SyncFromEntity()
    {
        IsOrdered = _entity.IsOrdered;
        Wait = (decimal)_entity.Wait;
    }

    public void Dispose() => _stack.Changed -= SyncFromEntity;
}
