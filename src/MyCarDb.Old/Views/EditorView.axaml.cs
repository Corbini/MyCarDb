using Avalonia.Controls;

namespace MyCarDbView.Views;

public partial class EditorView : UserControl, ITitleBarContent
{
    public EditorView()
    {
        InitializeComponent();
    }

    public Control? TitleBarLeading { get; set; }
    public Control? TitleBarCenter { get; set; }
    public Control? TitleBarTrailing { get; set; }
}
