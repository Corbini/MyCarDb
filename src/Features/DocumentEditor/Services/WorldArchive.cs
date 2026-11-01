using DocumentEditor.Models;
using GameWorld;
using GameWorld.Models;
using SaveFile;
using SaveFile.Values;

namespace DocumentEditor.Services;

public class WorldArchive(IFileService _fileService, IFileParser _fileParser, IWorldCoder _worldCoder) : IWorldArchive
{
    public async Task<ArchiveResult> LoadWorldAsync(string path)
    {
        DateTime lastModified;
        byte[] fileContent;
        try
        {
            lastModified = _fileService.GetFileModificationTime(path);
            fileContent = await _fileService.ReadFileAsync(path);
        }
        catch
        {
            return new ArchiveResult.Failed("WorldArchive_FileReadFailed");
        }

        List<IFileValue> fileValues;
        try
        {
            fileValues = _fileParser.Parse(fileContent);
        }
        catch
        {
            return new ArchiveResult.Failed("WorldArchive_ParseFailed");
        }

        try
        {
            var world = _worldCoder.Decode(fileValues);
            return new ArchiveResult.Success(world, lastModified);
        }
        catch
        {
            return new ArchiveResult.Failed("WorldArchive_DecodeFailed");
        }
    }

    public async Task<bool> SaveWorldAsync(World world, string path)
    {
        try
        {
            var fileValues = _worldCoder.Encode(world);
            byte[] fileContent = _fileParser.Serialize(fileValues);
            await _fileService.WriteFileAsync(path, fileContent);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
