using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MyCarDbView.Models.Alerts;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public sealed class AlertViewModel(IDialogService dialogService, AlertOptions options) : IViewModel<AlertOptions>
{
    public AlertOptions Options => options;

    public bool IsInformational => options.Severity == AlertSeverity.Informational;

    public bool IsWarning => options.Severity == AlertSeverity.Warning;

    public bool IsCritical => options.Severity == AlertSeverity.Critical;

    public string IconSymbol => options.Severity switch
    {
        AlertSeverity.Informational => "info",
        AlertSeverity.Warning => "warning",
        AlertSeverity.Critical => "warning-octagon",
        _ => string.Empty,
    };

    public ICommand CloseCommand { get; } = new RelayCommand(dialogService.Close);
}
