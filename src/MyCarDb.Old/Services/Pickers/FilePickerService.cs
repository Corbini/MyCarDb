using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DocumentEditor.Models.Pickers;
using DocumentEditor.Services.Pickers;

namespace MyCarDbView.Services.Pickers;

public sealed class FilePickerService :
    IFileOpenPickerService,
    IFilesOpenPickerService,
    IFileSavePickerService,
    IFolderPickerService
{
    private readonly IStorageProvider _storageProvider;

    public FilePickerService()
    {
        var desktop = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var storageProvider = desktop?.MainWindow?.StorageProvider;

        if (storageProvider is null)
            throw new InvalidOperationException("Avalonia main window is not initialized.");

        _storageProvider = storageProvider;
    }

    async Task<string?> IFileOpenPickerService.PickAsync(FileOpenPickerOptions options)
    {
        var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = false,
            FileTypeFilter = BuildFileTypes(options.FileTypes),
            SuggestedStartLocation = await TryGetFolder(options.SuggestedFolder),
        });

        return files.FirstOrDefault()?.Path.LocalPath;
    }

    async Task<IReadOnlyList<string>> IFilesOpenPickerService.PickAsync(FileOpenPickerOptions options)
    {
        var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = true,
            FileTypeFilter = BuildFileTypes(options.FileTypes),
            SuggestedStartLocation = await TryGetFolder(options.SuggestedFolder),
        });

        return files.Select(f => f.Path.LocalPath).ToArray();
    }

    async Task<string?> IFileSavePickerService.PickAsync(FileSavePickerOptions options)
    {
        var file = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = options.Title,
            SuggestedFileName = options.SuggestedFileName,
            FileTypeChoices = BuildFileTypes(options.FileTypes),
            SuggestedStartLocation = await TryGetFolder(options.SuggestedFolder),
        });

        return file?.Path.LocalPath;
    }

    async Task<string?> IFolderPickerService.PickAsync(FolderPickerOptions options)
    {
        var folders = await _storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = false,
            SuggestedStartLocation = await TryGetFolder(options.SuggestedFolder),
        });

        return folders.FirstOrDefault()?.Path.LocalPath;
    }

    private static IReadOnlyList<FilePickerFileType> BuildFileTypes(IReadOnlyList<FileTypeFilter> fileTypes)
    {
        return fileTypes
            .Select(ft => new FilePickerFileType(ft.Name) { Patterns = ft.Patterns.ToArray() })
            .ToArray();
    }

    private async Task<IStorageFolder?> TryGetFolder(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        return await _storageProvider.TryGetFolderFromPathAsync(path);
    }
}
