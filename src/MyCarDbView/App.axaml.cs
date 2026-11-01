using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Contracts;
using DocumentEditor.Dto;
using DocumentEditor.Services;
using DocumentEditor.Services.Pickers;
using DocumentEditor.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MyCarDbView.Dto;
using MyCarDbView.Localizations;
using MyCarDbView.Services;
using MyCarDbView.Services.Pickers;
using MyCarDbView.ViewModels;
using MyCarDbView.Views;
using SaveFile;

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
        services.AddSingleton<ILocalizationSource>(LocalizationSource.Instance);
        services.AddSingleton<IShellService, ShellService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ILicenseService, LicenseService>();
        services.AddSingleton<ILanguageService, LanguageService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<FilePickerService>();
        services.AddSingleton<IFileOpenPickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IFilesOpenPickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IFileSavePickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IFolderPickerService>(sp => sp.GetRequiredService<FilePickerService>());
        services.AddSingleton<IWritableOptions<AppSettings>>(_ =>
            new WritableOptions<AppSettings>(AppPaths.AppSettingsFile, AppJsonContext.Default.AppSettings));
        services.AddSingleton<IWritableOptions<RecentFiles>>(_ =>
            new WritableOptions<RecentFiles>(AppPaths.RecentFilesFile, AppJsonContext.Default.RecentFiles));
        services.AddSingleton<IWritableOptions<PinnedEntities>>(_ =>
            new WritableOptions<PinnedEntities>(AppPaths.PinnedEntitiesFile, AppJsonContext.Default.PinnedEntities));
        services.AddSingleton<IWritableOptions<BackupOptions>>(_ =>
            new WritableOptions<BackupOptions>(AppPaths.BackupFile, AppJsonContext.Default.BackupOptions));
        services.AddSingleton<IFileParser, FileParser>();
        services.AddGameWorld();
        services.AddDocumentEditor();
        return services.BuildServiceProvider();
    }
}
