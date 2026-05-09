using System.Text.Json;
using MyCarDbView.Dto;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

internal sealed class LastOpenedService : ILastOpenedService
{
    private readonly RecentFiles _state;

    public IReadOnlyList<LastOpenedFile> Recent => _state.Files;

    public LastOpenedService()
    {
        _state = TryLoad() ?? new RecentFiles();
    }

    public void Add(LastOpenedFile file)
    {
        _state.Files.RemoveAll(f => f.Path == file.Path);

        _state.Files.Add(file);
        Persist();
    }

    public void Remove(string path)
    {
        _state.Files.RemoveAll(f => f.Path == path);
        Persist();
    }

    private static RecentFiles? TryLoad()
    {
        try
        {
            string content = File.ReadAllText(AppPaths.RecentFilesFile);
            return JsonSerializer.Deserialize(content, AppJsonContext.Default.RecentFiles);
        }
        catch
        {
            return null;
        }
    }

    private void Persist()
    {
        AppPaths.EnsureExists();
        string json = JsonSerializer.Serialize(_state, AppJsonContext.Default.RecentFiles);
        File.WriteAllText(AppPaths.RecentFilesFile, json);
    }
}
