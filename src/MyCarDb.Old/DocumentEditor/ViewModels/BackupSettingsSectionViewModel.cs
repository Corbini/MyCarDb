using CommunityToolkit.Mvvm.ComponentModel;
using Contracts;
using DocumentEditor.Services;

namespace DocumentEditor.ViewModels;

public sealed partial class BackupSettingsSectionViewModel(IBackupService backupService) : ObservableObject, ISettingsSection
{
    [ObservableProperty]
    private bool _isAutoBackupEnabled = backupService.BackupSettings.IsAutoBackupEnabled;

    [ObservableProperty]
    private bool _isFileMonitorEnabled = backupService.BackupSettings.IsFileMonitorEnabled;

    partial void OnIsAutoBackupEnabledChanged(bool value) => backupService.SetAutoBackup(value);

    partial void OnIsFileMonitorEnabledChanged(bool value) => backupService.SetFileMonitor(value);
}
