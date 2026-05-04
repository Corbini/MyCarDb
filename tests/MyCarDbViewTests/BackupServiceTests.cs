using Moq;
using MyCarDbView.Dto;
using MyCarDbView.Models.AppState;
using MyCarDbView.Services;
using Xunit;

namespace MyCarDbViewTests.Services;

public class BackupServiceTests
{
    private static Mock<IWritableOptions<AppSettings>> CreateSettingsMock(AppSettings? initial = null)
    {
        var current = initial ?? new AppSettings();
        var mock = new Mock<IWritableOptions<AppSettings>>();
        mock.SetupGet(s => s.Value).Returns(() => current);
        mock.Setup(s => s.Update(It.IsAny<Action<AppSettings>>()))
            .Callback<Action<AppSettings>>(action => action(current));
        return mock;
    }

    [Fact]
    public void Constructor_ReadsBackupFromSettings()
    {
        var settings = CreateSettingsMock(new AppSettings
        {
            Backup = new BackupSettings(IsAutoBackupEnabled: false, IsFileMonitorEnabled: false),
        });

        var service = new BackupService(settings.Object);

        Assert.False(service.BackupSettings.IsAutoBackupEnabled);
        Assert.False(service.BackupSettings.IsFileMonitorEnabled);
    }

    [Fact]
    public void SetAutoBackup_WhenSameValue_DoesNotCallUpdate()
    {
        var settings = CreateSettingsMock(new AppSettings
        {
            Backup = new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true),
        });
        var service = new BackupService(settings.Object);

        service.SetAutoBackup(true);

        settings.Verify(s => s.Update(It.IsAny<Action<AppSettings>>()), Times.Never);
    }

    [Fact]
    public void SetAutoBackup_WhenDifferent_PersistsViaUpdate_AndKeepsFileMonitor()
    {
        var settings = CreateSettingsMock(new AppSettings
        {
            Backup = new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true),
        });
        var service = new BackupService(settings.Object);

        service.SetAutoBackup(false);

        Assert.False(service.BackupSettings.IsAutoBackupEnabled);
        Assert.True(service.BackupSettings.IsFileMonitorEnabled);
        Assert.False(settings.Object.Value.Backup.IsAutoBackupEnabled);
        Assert.True(settings.Object.Value.Backup.IsFileMonitorEnabled);
        settings.Verify(s => s.Update(It.IsAny<Action<AppSettings>>()), Times.Once);
    }

    [Fact]
    public void SetFileMonitor_WhenDifferent_PersistsViaUpdate_AndKeepsAutoBackup()
    {
        var settings = CreateSettingsMock(new AppSettings
        {
            Backup = new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true),
        });
        var service = new BackupService(settings.Object);

        service.SetFileMonitor(false);

        Assert.True(service.BackupSettings.IsAutoBackupEnabled);
        Assert.False(service.BackupSettings.IsFileMonitorEnabled);
        Assert.True(settings.Object.Value.Backup.IsAutoBackupEnabled);
        Assert.False(settings.Object.Value.Backup.IsFileMonitorEnabled);
        settings.Verify(s => s.Update(It.IsAny<Action<AppSettings>>()), Times.Once);
    }
}
