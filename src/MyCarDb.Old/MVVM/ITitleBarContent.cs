using Avalonia.Controls;

public interface ITitleBarContent
{
    public Control? TitleBarLeading { get; }
    public Control? TitleBarCenter { get; }
    public Control? TitleBarTrailing { get; }
}
