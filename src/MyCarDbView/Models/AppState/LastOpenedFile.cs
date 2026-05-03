namespace MyCarDbView.Models.AppState;

public sealed record LastOpenedFile(string Path, System.DateTime LastOpenedAt)
{
    public string Name => System.IO.Path.GetFileName(Path);
}
