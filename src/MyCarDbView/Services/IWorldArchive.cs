using GameWorld.Models;
using MyCarDbView.Models;

namespace MyCarDbView.Services;

public interface IWorldArchive
{
    Task<ArchiveResult> LoadWorldAsync(string path);
    Task<bool> SaveWorldAsync(World world, string path);
}
