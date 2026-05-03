using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyCarDbView.Services;

namespace MyCarDbView.MVVM;

public sealed class ViewLocator : IDataTemplate
{
    public Control? Build(IViewModel viewModel) => viewModel switch
    {
        _ => null,
    };

    Control? ITemplate<object?, Control?>.Build(object? data) =>
        data is IViewModel vm ? Build(vm) : null;

    bool IDataTemplate.Match(object? data) => data is IViewModel;
}
