using DocumentEditor.Models;
using GameWorld.Models;

namespace DocumentEditor.Services;

public interface IWorldArchive
{
    Task<ArchiveResult> LoadWorldAsync(string path);
    Task<bool> SaveWorldAsync(World world, string path);
}
