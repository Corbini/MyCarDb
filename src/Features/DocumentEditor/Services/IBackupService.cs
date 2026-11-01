using DocumentEditor.Models.AppState;

namespace DocumentEditor.Services;

public interface IBackupService
{
    BackupSettings BackupSettings { get; }

    void SetAutoBackup(bool enabled);

    void SetFileMonitor(bool enabled);
}
