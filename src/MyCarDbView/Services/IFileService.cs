namespace MyCarDbView.Services;

public interface IFileService
{
    public Task<byte[]> ReadFileAsync(string path);
    public Task WriteFileAsync(string path, byte[] content);

    public IDisposable WatchFile(string path, Action onChanged);
    public DateTime GetFileModificationTime(string path);
}
