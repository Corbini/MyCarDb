using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using AvaloniaStyles.Generated;

namespace AvaloniaStyles.Controls;

public class Icon : TemplatedControl
{
    public static readonly StyledProperty<string?> SymbolProperty =
        AvaloniaProperty.Register<Icon, string?>(nameof(Symbol));

    public static readonly StyledProperty<Geometry?> DataProperty =
        AvaloniaProperty.Register<Icon, Geometry?>(nameof(Data));

    public static readonly StyledProperty<Rect> ViewBoxProperty =
        AvaloniaProperty.Register<Icon, Rect>(nameof(ViewBox), new Rect(0, 0, 256, 256));

    static Icon()
    {
        SymbolProperty.Changed.AddClassHandler<Icon>((icon, _) => icon.UpdateFromSymbol());
    }

    public string? Symbol
    {
        get => GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    public Geometry? Data
    {
        get => GetValue(DataProperty);
        private set => SetValue(DataProperty, value);
    }

    public Rect ViewBox
    {
        get => GetValue(ViewBoxProperty);
        private set => SetValue(ViewBoxProperty, value);
    }

    private void UpdateFromSymbol()
    {
        if (Symbol is null || !IconCatalog.All.TryGetValue(Symbol, out var def))
        {
            Data = null;
            return;
        }
        Data = def.Geometry;
        ViewBox = def.ViewBox;
    }
}
