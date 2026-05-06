using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyCarDb;
using MyCarDbView.Dto;
using MyCarDbView.Localizations;
using MyCarDbView.Services;
using MyCarDbView.ViewModels;
using MyCarDbView.Views;
using MyCarDbWorld;

namespace MyCarDbView;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = BuildServices();

        services.GetRequiredService<ILanguageService>();
        services.GetRequiredService<IThemeService>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Exit += (_, _) => (services as IDisposable)?.Dispose();
            var window = ActivatorUtilities.CreateInstance<MainWindow>(services);
            desktop.MainWindow = window;
            var windowViewModel = ActivatorUtilities.CreateInstance<MainWindowViewModel>(services);
            window.DataContext = windowViewModel;

            var shell = services.GetRequiredService<IShellService>();
            shell.NavigateTo<WelcomeViewModel>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton(Localizer.View);
        services.AddSingleton(Localizer.Db);
        services.AddSingleton<IShellService, ShellService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ILicenseService, LicenseService>();
        services.AddSingleton<ILanguageService, LanguageService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IBackupService, BackupService>();
        services.AddSingleton<ISaveFolderResolver, SaveFolderResolver>();
        services.AddSingleton<FilePickerService>();
        services.AddSingleton<IFileOpenPickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IFilesOpenPickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IFileSavePickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IFolderPickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IWritableOptions<AppSettings>>(_ =>
            new WritableOptions<AppSettings>(AppPaths.AppSettingsFile, AppJsonContext.Default.AppSettings));
        services.AddSingleton<ILastOpenedService, LastOpenedService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IWorldArchive, WorldArchive>();
        services.AddSingleton<IFileParser, FileParser>();
        services.AddSingleton<IWorldCoder, WorldCoder>();
        return services.BuildServiceProvider();
    }
}
