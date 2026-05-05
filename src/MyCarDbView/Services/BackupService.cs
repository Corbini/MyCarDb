using MyCarDbView.Dto;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

internal sealed class BackupService(IWritableOptions<AppSettings> settings) : IBackupService
{
    public BackupSettings BackupSettings { get; private set; } = settings.Value.Backup;

    public void SetAutoBackup(bool enabled)
    {
        if (BackupSettings.IsAutoBackupEnabled == enabled)
            return;

        BackupSettings = BackupSettings with { IsAutoBackupEnabled = enabled };
        settings.Update(s => s.Backup = BackupSettings);
    }

    public void SetFileMonitor(bool enabled)
    {
        if (BackupSettings.IsFileMonitorEnabled == enabled)
            return;

        BackupSettings = BackupSettings with { IsFileMonitorEnabled = enabled };
        settings.Update(s => s.Backup = BackupSettings);
    }
}
