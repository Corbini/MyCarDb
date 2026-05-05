namespace MyCarDbView.Services;

internal sealed class SaveFolderResolver : ISaveFolderResolver
{
    public string? SuggestedFolder
    {
        get
        {
            foreach (string candidate in EnumerateCandidatePaths())
            {
                if (Directory.Exists(candidate))
                    return candidate;
            }

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return !string.IsNullOrEmpty(documents) && Directory.Exists(documents)
                ? documents
                : null;
        }
    }

    private static IEnumerable<string> EnumerateCandidatePaths()
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (OperatingSystem.IsWindows())
        {
            yield return Path.Combine(home, "AppData", "LocalLow", "Amistech");
            yield break;
        }

        if (OperatingSystem.IsLinux())
        {
            string[] amistechAppIds = ["516750", "4164420"];   // My Summer Car, My Winter Car
            string steamRoot = Path.Combine(home, ".steam", "steam");
            string protonSub = Path.Combine("pfx", "drive_c", "users", "steamuser", "AppData", "LocalLow", "Amistech");

            foreach (string appId in amistechAppIds)
                yield return Path.Combine(steamRoot, "steamapps", "compatdata", appId, protonSub);
        }
    }
}
