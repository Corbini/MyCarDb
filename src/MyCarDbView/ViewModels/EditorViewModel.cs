using CommunityToolkit.Mvvm.ComponentModel;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public record EditorParameters(string FilePath);

public sealed partial class EditorViewModel(
    EditorParameters parameters)
    : ObservableObject, IViewModel<EditorParameters>
{
    public string FilePath { get; } = parameters.FilePath;
}
