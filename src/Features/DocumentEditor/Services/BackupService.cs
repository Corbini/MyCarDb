using Contracts;
using DocumentEditor.Dto;
using DocumentEditor.Models.AppState;

namespace DocumentEditor.Services;

internal sealed class BackupService(IWritableOptions<BackupOptions> options) : IBackupService
{
    public BackupSettings BackupSettings { get; private set; } = options.Value.Settings;

    public void SetAutoBackup(bool enabled)
    {
        if (BackupSettings.IsAutoBackupEnabled == enabled)
            return;

        BackupSettings = BackupSettings with { IsAutoBackupEnabled = enabled };
        options.Update(o => o.Settings = BackupSettings);
    }

    public void SetFileMonitor(bool enabled)
    {
        if (BackupSettings.IsFileMonitorEnabled == enabled)
            return;

        BackupSettings = BackupSettings with { IsFileMonitorEnabled = enabled };
        options.Update(o => o.Settings = BackupSettings);
    }
}
