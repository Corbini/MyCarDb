using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyCarDbView.Items;

public sealed partial class MapItem : ObservableObject
{
    public required EntityItem Value { get; init; }

    public required double X { get; init; }
    public required double Z { get; init; }

    [ObservableProperty]
    private bool _isSelected;
}
