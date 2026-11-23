using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using GameWorld.Commands;
using GameWorld.Models;

namespace DocumentEditor.ViewModels;

public sealed partial class PropertyEntityInspectorViewModel : ObservableObject, IDisposable
{
    private readonly PropertyEntity _entity;
    private readonly ICommandExecutor _stack;

    [ObservableProperty]
    private decimal _positionX;

    [ObservableProperty]
    private decimal _positionY;

    public PropertyEntityInspectorViewModel(
        ICommandExecutor stack,
        PropertyEntity entity)
    {
        _stack = stack;
        _entity = entity;
        _positionX = (decimal)entity.Position.X;
        _positionY = (decimal)entity.Position.Y;
        _stack.Changed += SyncFromEntity;
    }

    partial void OnPositionXChanged(decimal value)
        => Commit(new Vector2((float)value, (float)PositionY));

    partial void OnPositionYChanged(decimal value)
        => Commit(new Vector2((float)PositionX, (float)value));

    private void Commit(Vector2 newPosition)
    {
        if (newPosition == _entity.Position)
            return;
        _stack.Execute(
            new SetPropertyPositionCommand(_entity.Id, _entity.Position, newPosition));
    }

    private void SyncFromEntity()
    {
        PositionX = (decimal)_entity.Position.X;
        PositionY = (decimal)_entity.Position.Y;
    }

    public void Dispose()
        => _stack.Changed -= SyncFromEntity;
}
