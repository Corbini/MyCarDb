using Contracts;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public class MainWindowViewModel(IShellService shellService, IDialogService dialogService)
{
    public IShellService ShellService { get; } = shellService;
    public IDialogService DialogService { get; } = dialogService;
}
