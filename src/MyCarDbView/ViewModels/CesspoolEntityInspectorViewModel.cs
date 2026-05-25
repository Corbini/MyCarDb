using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbWorld.Commands;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public sealed partial class CesspoolEntityInspectorViewModel : ObservableObject, IDisposable
{
    private readonly CesspoolEntity _entity;
    private readonly ICommandExecutor _stack;
    private bool _isSyncing;

    [ObservableProperty]
    private decimal _level;

    [ObservableProperty]
    private decimal _rate;

    [ObservableProperty]
    private bool _isPickupOrdered;

    public CesspoolEntityInspectorViewModel(
        ICommandExecutor stack,
        CesspoolEntity entity)
    {
        _stack = stack;
        _entity = entity;
        _isSyncing = true;
        Level = (decimal)entity.Level;
        Rate = (decimal)entity.Rate;
        IsPickupOrdered = entity.IsPickupOrdered;
        _isSyncing = false;
        _stack.Changed += SyncFromEntity;
    }

    partial void OnLevelChanged(decimal value)
    {
        if (_isSyncing)
            return;
        var newLevel = (float)value;
        if (newLevel == _entity.Level)
            return;
        _stack.Execute(new SetCesspoolLevelCommand(_entity.Id, _entity.Level, newLevel));
    }

    partial void OnRateChanged(decimal value)
    {
        if (_isSyncing)
            return;
        var newRate = (float)value;
        if (newRate == _entity.Rate)
            return;
        _stack.Execute(new SetCesspoolRateCommand(_entity.Id, _entity.Rate, newRate));
    }

    partial void OnIsPickupOrderedChanged(bool value)
    {
        if (_isSyncing)
            return;
        if (value == _entity.IsPickupOrdered)
            return;
        _stack.Execute(new SetCesspoolPickupCommand(_entity.Id, _entity.IsPickupOrdered, value));
    }

    private void SyncFromEntity()
    {
        _isSyncing = true;
        Level = (decimal)_entity.Level;
        Rate = (decimal)_entity.Rate;
        IsPickupOrdered = _entity.IsPickupOrdered;
        _isSyncing = false;
    }

    public void Dispose()
        => _stack.Changed -= SyncFromEntity;
}