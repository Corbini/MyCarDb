using Avalonia.Controls;

namespace MyCarDbView.Views;

public partial class WelcomeView : UserControl, ITitleBarContent
{
    public WelcomeView()
    {
        InitializeComponent();
    }

    public Control? TitleBarLeading { get; set; }
    public Control? TitleBarCenter { get; set; }
    public Control? TitleBarTrailing { get; set; }
}
