using Contracts;
using DocumentEditor.Services;
using DocumentEditor.ViewModels;

namespace Microsoft.Extensions.DependencyInjection;

public static class DocumentEditorDependencyInjection
{
    public static IServiceCollection AddDocumentEditor(this IServiceCollection services)
    {
        services.AddSingleton<IBackupService, BackupService>();
        services.AddSingleton<ISaveFolderResolver, SaveFolderResolver>();
        services.AddSingleton<ILastOpenedService, LastOpenedService>();
        services.AddSingleton<IPinnedEntitiesService, PinnedEntitiesService>();
        services.AddSingleton<IMapItemFactory, MapItemFactory>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IWorldArchive, WorldArchive>();
        services.AddSingleton<IDocumentManager, DocumentManager>();
        services.AddTransient<IEntityBrowserViewModel, EntityBrowserViewModel>();
        services.AddTransient<IInspectorViewModel, InspectorViewModel>();
        services.AddTransient<IMapViewModel, MapViewModel>();
        services.AddTransient<ISettingsSection, BackupSettingsSectionViewModel>();
        return services;
    }
}
