using MyCarDbView.Models.AppState;

namespace MyCarDbView.Services;

public interface IBackupService
{
    BackupSettings BackupSettings { get; }

    void SetAutoBackup(bool enabled);

    void SetFileMonitor(bool enabled);
}
