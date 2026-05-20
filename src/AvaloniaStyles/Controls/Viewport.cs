using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaStyles.Controls;

public class Viewport : ContentControl
{
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

    private Point? _dragStart;
    private double _dragStartOffsetX;
    private double _dragStartOffsetY;

    public ICommand ZoomInCommand { get; }

    public ICommand ZoomOutCommand { get; }

    public Viewport()
    {
        ZoomInCommand = new RelayCommand(() => Zoom *= ZoomStep);
        ZoomOutCommand = new RelayCommand(() => Zoom /= ZoomStep);
    }

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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == BoundsProperty)
        {
            CoerceValue(OffsetXProperty);
            CoerceValue(OffsetYProperty);
        }
        else if (change.Property == ZoomProperty
            && change.OldValue is double zoomValue)
        {
            OnScrollChanged(zoomValue);
        }
        else if (change.Property == MinZoomProperty
            || change.Property == MaxZoomProperty)
        {
            CoerceValue(ZoomProperty);
        }
    }

    private void OnScrollChanged(double oldZoom)
    {
        if (Math.Abs(oldZoom) <= 0.0001)
            return;

        double k = Zoom / oldZoom;
        double newOffsetX;
        double newOffsetY;

        if (_scrollMouseCenter is Point point)
        {
            newOffsetX = (point.X - Bounds.Width / 2) * (1 - k) + OffsetX * k;
            newOffsetY = (point.Y - Bounds.Height / 2) * (1 - k) + OffsetY * k;
            _scrollMouseCenter = null;
        }
        else
        {
            newOffsetX = OffsetX * k;
            newOffsetY = OffsetY * k;
        }

        OffsetX = newOffsetX;
        OffsetY = newOffsetY;
    }

    private Point? _scrollMouseCenter;
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        bool zoomingIn = e.Delta.Y > 0;
        bool canZoom = zoomingIn ? Zoom < MaxZoom : Zoom > MinZoom;

        if (canZoom)
        {
            _scrollMouseCenter = e.GetPosition(this);
            Zoom = zoomingIn ? Zoom * ZoomStep : Zoom / ZoomStep;
        }

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
        Cursor = new Cursor(StandardCursorType.SizeAll);
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
