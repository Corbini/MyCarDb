using Contracts;
using DocumentEditor.Dto;
using DocumentEditor.Models.AppState;

namespace DocumentEditor.Services;

internal sealed class LastOpenedService(IWritableOptions<RecentFiles> settings) : ILastOpenedService
{
    public IReadOnlyList<LastOpenedFile> Recent => settings.Value.Files;

    public void Add(LastOpenedFile file) =>
        settings.Update(s =>
        {
            s.Files.RemoveAll(f => f.Path == file.Path);
            s.Files.Insert(0, file);
        });

    public void Remove(string path) =>
        settings.Update(s => s.Files.RemoveAll(f => f.Path == path));
}
