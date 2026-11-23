using Contracts;
using DocumentEditor.Services;
using DocumentEditor.ViewModels;

namespace Microsoft.Extensions.DependencyInjection;

public static class DocumentEditorViewModelsDependencyInjection
{
    public static IServiceCollection AddDocumentEditorViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IMapItemFactory, MapItemFactory>();
        services.AddTransient<IEntityBrowserViewModel, EntityBrowserViewModel>();
        services.AddTransient<IInspectorViewModel, InspectorViewModel>();
        services.AddTransient<IMapViewModel, MapViewModel>();
        services.AddTransient<ISettingsSection, BackupSettingsSectionViewModel>();
        return services;
    }
}
