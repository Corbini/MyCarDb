using System.Windows.Input;
using AvaloniaStyles.Generated;
using CommunityToolkit.Mvvm.Input;
using Contracts;
using MyCarDbView.Models.Alerts;
using MyCarDbView.Services;

namespace MyCarDbView.ViewModels;

public sealed class AlertViewModel(IDialogService dialogService, AlertOptions options) : IViewModel<AlertOptions>
{
    public AlertOptions Options => options;

    public bool IsInformational => options.Severity == AlertSeverity.Informational;

    public bool IsWarning => options.Severity == AlertSeverity.Warning;

    public bool IsCritical => options.Severity == AlertSeverity.Critical;

    public SvgSymbol? IconSymbol => options.Severity switch
    {
        AlertSeverity.Informational => SvgSymbol.Info,
        AlertSeverity.Warning => SvgSymbol.Warning,
        AlertSeverity.Critical => SvgSymbol.WarningOctagon,
        _ => null,
    };

    public ICommand CloseCommand { get; } = new RelayCommand(dialogService.Close);
}
