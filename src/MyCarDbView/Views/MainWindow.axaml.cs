using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MyCarDbView.ViewModels;

namespace MyCarDbView.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ShellHost.PropertyChanged += (_, e) =>
        {
            if (e.Property == ContentControl.ContentProperty)
                Dispatcher.UIThread.Post(UpdateTitleBarSlots, DispatcherPriority.Background);
        };

        PropertyChanged += (_, e) =>
        {
            if (e.Property == WindowStateProperty)
                ResizeHandles.IsHitTestVisible = WindowState != WindowState.Maximized;
        };

        Loaded += (_, _) =>
        {
            UpdateTitleBarSlots();
            ResizeHandles.IsHitTestVisible = WindowState != WindowState.Maximized;
        };
    }

    private void UpdateTitleBarSlots()
    {
        var tbc = ShellHost.Presenter?.Child as ITitleBarContent;
        TitleBarLeading.Content = tbc?.TitleBarLeading;
        TitleBarCenter.Content = tbc?.TitleBarCenter;
        TitleBarTrailing.Content = tbc?.TitleBarTrailing;
    }

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Visual v && (v is Button || v.FindAncestorOfType<Button>() is not null))
            return;

        var props = e.GetCurrentPoint(this).Properties;
        if (!props.IsLeftButtonPressed)
            return;

        if (e.ClickCount == 2)
            ToggleMaximize();
        else
            BeginMoveDrag(e);
    }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void OnMaximizeClick(object? sender, RoutedEventArgs e)
        => ToggleMaximize();

    private void OnCloseClick(object? sender, RoutedEventArgs e)
        => Close();

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Handled || e.Key != Key.Escape)
            return;
        if (DataContext is not MainWindowViewModel vm)
            return;
        if (vm.DialogService.CurrentContent is null)
            return;
        vm.DialogService.Close();
        e.Handled = true;
    }

    private void ToggleMaximize()
        => WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void OnResizeNorth(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.North, e);
    private void OnResizeSouth(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.South, e);
    private void OnResizeWest(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.West, e);
    private void OnResizeEast(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.East, e);
    private void OnResizeNorthWest(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.NorthWest, e);
    private void OnResizeNorthEast(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.NorthEast, e);
    private void OnResizeSouthWest(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.SouthWest, e);
    private void OnResizeSouthEast(object? sender, PointerPressedEventArgs e) => BeginResize(WindowEdge.SouthEast, e);

    private void BeginResize(WindowEdge edge, PointerPressedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
            return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;
        BeginResizeDrag(edge, e);
    }
}
