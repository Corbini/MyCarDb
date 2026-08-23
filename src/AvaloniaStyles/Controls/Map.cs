using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using AvaloniaStyles.Generated;

namespace AvaloniaStyles.Controls;

public sealed record MapFigure(Geometry Geometry, IBrush? Brush);

public class Map : TemplatedControl
{
    public static readonly StyledProperty<SvgSymbol?> SymbolProperty =
        AvaloniaProperty.Register<Map, SvgSymbol?>(nameof(Symbol));

    public static readonly StyledProperty<IReadOnlyList<MapFigure>?> FiguresProperty =
        AvaloniaProperty.Register<Map, IReadOnlyList<MapFigure>?>(nameof(Figures));

    public static readonly StyledProperty<Rect> ViewBoxProperty =
        AvaloniaProperty.Register<Map, Rect>(nameof(ViewBox), new Rect(0, 0, 1024, 1024));

    static Map()
    {
        SymbolProperty.Changed.AddClassHandler<Map>((map, _) => map.UpdateFromSymbol());
    }

    public SvgSymbol? Symbol
    {
        get => GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    public IReadOnlyList<MapFigure>? Figures
    {
        get => GetValue(FiguresProperty);
        private set => SetValue(FiguresProperty, value);
    }

    public Rect ViewBox
    {
        get => GetValue(ViewBoxProperty);
        private set => SetValue(ViewBoxProperty, value);
    }

    private void UpdateFromSymbol()
    {
        if (Symbol is null || !SvgCatalog.All.TryGetValue(Symbol.Value, out var def))
        {
            Figures = null;
            return;
        }

        var figures = new MapFigure[def.Figures.Count];
        for (int i = 0; i < def.Figures.Count; i++)
        {
            var f = def.Figures[i];
            IBrush? brush = f.FillArgb is uint argb
                ? new ImmutableSolidColorBrush(Color.FromUInt32(argb))
                : null;
            figures[i] = new MapFigure(f.Geometry, brush);
        }
        Figures = figures;
        ViewBox = def.ViewBox;
    }
}
