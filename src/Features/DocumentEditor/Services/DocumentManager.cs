using CommunityToolkit.Mvvm.ComponentModel;
using DocumentEditor.Models;
using GameWorld.Commands;

namespace DocumentEditor.Services;

internal sealed partial class DocumentManager(IWorldArchive archive) : ObservableObject, IDocumentManager
{
    [ObservableProperty]
    private FileSession? _activeDocument;

    public async Task<ArchiveResult> OpenFromPathAsync(string path)
    {
        var result = await archive.LoadWorldAsync(path);
        if (result is ArchiveResult.Success success)
        {
            ActiveDocument = new FileSession(
                success.Content,
                new CommandStack(success.Content),
                path);
        }
        return result;
    }
}

