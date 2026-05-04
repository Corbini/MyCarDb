using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

public interface ILastOpenedService
{
    IReadOnlyList<LastOpenedFile> Recent { get; }

    void Add(LastOpenedFile file);

    void Remove(string path);
}
