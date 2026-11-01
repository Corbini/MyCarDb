using DocumentEditor.Models.AppState;

namespace DocumentEditor.Services;

public interface ILastOpenedService
{
    IReadOnlyList<LastOpenedFile> Recent { get; }

    void Add(LastOpenedFile file);

    void Remove(string path);
}
