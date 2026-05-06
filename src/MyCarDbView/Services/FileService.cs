namespace MyCarDbView.Services;

public class FileService : IFileService
{
    public Task<byte[]> ReadFileAsync(string path)
        => File.ReadAllBytesAsync(path);

    public Task WriteFileAsync(string path, byte[] content)
        => File.WriteAllBytesAsync(path, content);

    public IDisposable WatchFile(string path, Action onChanged)
    {
        var watcher = new FileSystemWatcher(Path.GetDirectoryName(path)!, Path.GetFileName(path)!);
        watcher.Changed += (s, e) => onChanged();
        watcher.Renamed += (s, e) => onChanged();
        watcher.Deleted += (s, e) => onChanged();
        watcher.EnableRaisingEvents = true;
        return watcher;
    }

    public DateTime GetFileModificationTime(string path)
        => File.GetLastWriteTime(path);
}
