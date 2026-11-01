namespace MyCarDbView.Services;

internal static class AppPaths
{
    public static string AppDataDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MyCarDb");

    public static string AppSettingsFile { get; } = Path.Combine(AppDataDirectory, "appsettings.json");

    public static string RecentFilesFile { get; } = Path.Combine(AppDataDirectory, "recent.json");
    public static string PinnedEntitiesFile { get; } = Path.Combine(AppDataDirectory, "pinned.json");
    public static string BackupFile { get; } = Path.Combine(AppDataDirectory, "backup.json");

    public static void EnsureExists() => Directory.CreateDirectory(AppDataDirectory);
}
