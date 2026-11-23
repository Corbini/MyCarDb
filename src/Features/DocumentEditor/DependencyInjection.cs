using DocumentEditor.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class DocumentEditorDependencyInjection
{
    public static IServiceCollection AddDocumentEditor(this IServiceCollection services)
    {
        services.AddSingleton<IBackupService, BackupService>();
        services.AddSingleton<ISaveFolderResolver, SaveFolderResolver>();
        services.AddSingleton<ILastOpenedService, LastOpenedService>();
        services.AddSingleton<IPinnedEntitiesService, PinnedEntitiesService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IWorldArchive, WorldArchive>();
        services.AddSingleton<IDocumentManager, DocumentManager>();
        return services;
    }
}
