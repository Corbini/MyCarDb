using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyCarDbView.Services;
using MyCarDbView.ViewModels;
using MyCarDbView.Views;

namespace MyCarDbView;

public partial class App : Application
{
    private IServiceProvider? _services;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _services = BuildServices();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Exit += (_, _) => (_services as IDisposable)?.Dispose();

            var shell = _services.GetRequiredService<IShellService>();
            shell.NavigateTo<WelcomeViewModel>();

            var windowViewModel = ActivatorUtilities.CreateInstance<MainWindowViewModel>(_services);
            var window = ActivatorUtilities.CreateInstance<MainWindow>(_services);
            window.DataContext = windowViewModel;

            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IShellService, ShellService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ILicenseService, LicenseService>();
        services.AddSingleton<ILanguageService, LanguageService>();
        services.AddSingleton<IThemeService, ThemeService>();
        return services.BuildServiceProvider();
    }
}
