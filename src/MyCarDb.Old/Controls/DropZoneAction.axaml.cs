using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace MyCarDbView.Controls;

public partial class DropZoneAction : UserControl
{
    public static readonly StyledProperty<object?> InnerContentProperty =
        AvaloniaProperty.Register<DropZoneAction, object?>(nameof(InnerContent));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DropZoneAction, string?>(nameof(Title));

    public static readonly StyledProperty<string?> CaptionProperty =
        AvaloniaProperty.Register<DropZoneAction, string?>(nameof(Caption));

    public static readonly StyledProperty<ICommand?> DropCommandProperty =
        AvaloniaProperty.Register<DropZoneAction, ICommand?>(nameof(DropCommand));

    public static readonly DirectProperty<DropZoneAction, bool> IsActiveProperty =
        AvaloniaProperty.RegisterDirect<DropZoneAction, bool>(
            nameof(IsActive),
            o => o.IsActive);

    private bool _isActive;

    public DropZoneAction()
    {
        InitializeComponent();
        DragDrop.SetAllowDrop(this, true);
        AddHandler(DragDrop.DragEnterEvent, OnDragEnter);
        AddHandler(DragDrop.DragLeaveEvent, OnDragLeave);
        AddHandler(DragDrop.DropEvent, OnDrop);
    }

    [Avalonia.Metadata.Content]
    public object? InnerContent
    {
        get => GetValue(InnerContentProperty);
        set => SetValue(InnerContentProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Caption
    {
        get => GetValue(CaptionProperty);
        set => SetValue(CaptionProperty, value);
    }

    public ICommand? DropCommand
    {
        get => GetValue(DropCommandProperty);
        set => SetValue(DropCommandProperty, value);
    }

    public bool IsActive
    {
        get => _isActive;
        private set => SetAndRaise(IsActiveProperty, ref _isActive, value);
    }

    private void OnDragEnter(object? sender, DragEventArgs e)
    {
        if (e.DataTransfer.Contains(DataFormat.File))
        {
            IsActive = true;
            e.DragEffects = DragDropEffects.Copy;
        }
    }

    private void OnDragLeave(object? sender, DragEventArgs e)
    {
        IsActive = false;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        IsActive = false;

        if (!e.DataTransfer.Contains(DataFormat.File))
        {
            return;
        }

        string? path = e.DataTransfer.TryGetFiles()?.FirstOrDefault()?.Path.LocalPath;
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        if (DropCommand?.CanExecute(path) == true)
        {
            DropCommand.Execute(path);
        }
    }
}
