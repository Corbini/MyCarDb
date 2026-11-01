using GameWorld.Models;

namespace MyCarDbView.Models;

public abstract record ArchiveResult
{
    private ArchiveResult() { }
    public sealed record Success(World Content, DateTime LastModified) : ArchiveResult;
    public sealed record Failed(string DetailKey) : ArchiveResult;
}
