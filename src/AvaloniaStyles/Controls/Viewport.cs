using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaStyles.Controls;

public class Viewport : ContentControl
{
    private const double _zoomEpsilon = 1e-4;
    private static readonly Cursor _dragCursor = new(StandardCursorType.SizeAll);

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<Viewport, double>(
            nameof(Zoom),
            defaultValue: 1.0,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay,
            coerce: CoerceZoom);

    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(MinZoom), defaultValue: 0.5);

    public static readonly StyledProperty<double> MaxZoomProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(MaxZoom), defaultValue: 8.0);

    public static readonly StyledProperty<double> ZoomStepProperty =
        AvaloniaProperty.Register<Viewport, double>(nameof(ZoomStep), defaultValue: 1.25);

    public static readonly StyledProperty<double> OffsetXProperty =
        AvaloniaProperty.Register<Viewport, double>(
            nameof(OffsetX),
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay,
            coerce: CoerceOffsetX);

    public static readonly StyledProperty<double> OffsetYProperty =
        AvaloniaProperty.Register<Viewport, double>(
            nameof(OffsetY),
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay,
            coerce: CoerceOffsetY);

    public static readonly StyledProperty<double> InverseZoomProperty =
        AvaloniaProperty.Register<Viewport, double>(
            nameof(InverseZoom), defaultValue: 1);

    static Viewport()
    {
        BoundsProperty.Changed.AddClassHandler<Viewport>((vp, _) =>
        {
            vp.CoerceValue(OffsetXProperty);
            vp.CoerceValue(OffsetYProperty);
        });
        ZoomProperty.Changed.AddClassHandler<Viewport>((vp, _) => vp.OnZoomChanged());
        MinZoomProperty.Changed.AddClassHandler<Viewport>((vp, _) => vp.OnZoomBoundsChanged());
        MaxZoomProperty.Changed.AddClassHandler<Viewport>((vp, _) => vp.OnZoomBoundsChanged());
    }

    public double Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public double MinZoom
    {
        get => GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    public double MaxZoom
    {
        get => GetValue(MaxZoomProperty);
        set => SetValue(MaxZoomProperty, value);
    }

    public double ZoomStep
    {
        get => GetValue(ZoomStepProperty);
        set => SetValue(ZoomStepProperty, value);
    }

    public double OffsetX
    {
        get => GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    public double OffsetY
    {
        get => GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }

    public double InverseZoom
    {
        get => GetValue(InverseZoomProperty);
        private set => SetValue(InverseZoomProperty, value);
    }

    private Point? _dragStart;
    private double _dragStartOffsetX;
    private double _dragStartOffsetY;

    private RelayCommand? _zoomInCommand;
    private RelayCommand? _zoomOutCommand;

    public ICommand ZoomInCommand => _zoomInCommand ??= new RelayCommand(
        () => ZoomBy(ZoomStep, null),
        () => Zoom < MaxZoom);

    public ICommand ZoomOutCommand => _zoomOutCommand ??= new RelayCommand(
        () => ZoomBy(1.0 / ZoomStep, null),
        () => Zoom > MinZoom);

    private static double CoerceZoom(AvaloniaObject obj, double value)
    {
        var vp = (Viewport)obj;
        return Math.Clamp(value, vp.MinZoom, vp.MaxZoom);
    }

    private static double CoerceOffsetX(AvaloniaObject obj, double value)
    {
        var vp = (Viewport)obj;
        double max = vp.Bounds.Width * vp.Zoom / 2;
        return Math.Clamp(value, -max, max);
    }

    private static double CoerceOffsetY(AvaloniaObject obj, double value)
    {
        var vp = (Viewport)obj;
        double max = vp.Bounds.Height * vp.Zoom / 2;
        return Math.Clamp(value, -max, max);
    }

    private void OnZoomChanged()
    {
        InverseZoom = Zoom > _zoomEpsilon ? 1.0 / Zoom : 1.0;
        _zoomInCommand?.NotifyCanExecuteChanged();
        _zoomOutCommand?.NotifyCanExecuteChanged();
    }

    private void OnZoomBoundsChanged()
    {
        CoerceValue(ZoomProperty);
        _zoomInCommand?.NotifyCanExecuteChanged();
        _zoomOutCommand?.NotifyCanExecuteChanged();
    }

    private void ZoomBy(double factor, Point? mouseCenter)
    {
        double oldZoom = Zoom;
        Zoom *= factor;
        if (Math.Abs(Zoom - oldZoom) <= _zoomEpsilon)
            return;
        AdjustOffsetForZoom(oldZoom, Zoom, mouseCenter);
    }

    private void AdjustOffsetForZoom(double oldZoom, double newZoom, Point? mouseCenter)
    {
        if (oldZoom <= _zoomEpsilon)
            return;

        double k = newZoom / oldZoom;
        var reference = mouseCenter ?? new Point(Bounds.Width / 2, Bounds.Height / 2);
        OffsetX = (reference.X - Bounds.Width / 2) * (1 - k) + OffsetX * k;
        OffsetY = (reference.Y - Bounds.Height / 2) * (1 - k) + OffsetY * k;
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        bool zoomingIn = e.Delta.Y > 0;
        ZoomBy(zoomingIn ? ZoomStep : 1.0 / ZoomStep, e.GetPosition(this));
        e.Handled = true;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.Handled)
            return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        _dragStart = e.GetPosition(this);
        _dragStartOffsetX = OffsetX;
        _dragStartOffsetY = OffsetY;
        e.Pointer.Capture(this);
        Cursor = _dragCursor;
        e.Handled = true;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (_dragStart is null)
            return;

        var current = e.GetPosition(this);
        OffsetX = _dragStartOffsetX + (current.X - _dragStart.Value.X);
        OffsetY = _dragStartOffsetY + (current.Y - _dragStart.Value.Y);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (_dragStart is null)
            return;

        _dragStart = null;
        e.Pointer.Capture(null);
        Cursor = Cursor.Default;
    }
}
