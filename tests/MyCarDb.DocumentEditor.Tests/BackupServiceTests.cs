using Contracts;
using DocumentEditor.Dto;
using DocumentEditor.Models.AppState;
using DocumentEditor.Services;
using Moq;
using Xunit;

namespace DocumentEditor.Tests.Services;

public class BackupServiceTests
{
    private static Mock<IWritableOptions<BackupOptions>> CreateSettingsMock(BackupOptions? initial = null)
    {
        var current = initial ?? new BackupOptions();
        var mock = new Mock<IWritableOptions<BackupOptions>>();
        mock.SetupGet(s => s.Value).Returns(() => current);
        mock.Setup(s => s.Update(It.IsAny<Action<BackupOptions>>()))
            .Callback<Action<BackupOptions>>(action => action(current));
        return mock;
    }

    [Fact]
    public void Constructor_ReadsBackupFromSettings()
    {
        var settings = CreateSettingsMock(new BackupOptions
        {
            Settings = new BackupSettings(IsAutoBackupEnabled: false, IsFileMonitorEnabled: false),
        });

        var service = new BackupService(settings.Object);

        Assert.False(service.BackupSettings.IsAutoBackupEnabled);
        Assert.False(service.BackupSettings.IsFileMonitorEnabled);
    }

    [Fact]
    public void SetAutoBackup_WhenSameValue_DoesNotCallUpdate()
    {
        var settings = CreateSettingsMock(new BackupOptions
        {
            Settings = new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true),
        });
        var service = new BackupService(settings.Object);

        service.SetAutoBackup(true);

        settings.Verify(s => s.Update(It.IsAny<Action<BackupOptions>>()), Times.Never);
    }

    [Fact]
    public void SetAutoBackup_WhenDifferent_PersistsViaUpdate_AndKeepsFileMonitor()
    {
        var settings = CreateSettingsMock(new BackupOptions
        {
            Settings = new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true),
        });
        var service = new BackupService(settings.Object);

        service.SetAutoBackup(false);

        Assert.False(service.BackupSettings.IsAutoBackupEnabled);
        Assert.True(service.BackupSettings.IsFileMonitorEnabled);
        Assert.False(settings.Object.Value.Settings.IsAutoBackupEnabled);
        Assert.True(settings.Object.Value.Settings.IsFileMonitorEnabled);
        settings.Verify(s => s.Update(It.IsAny<Action<BackupOptions>>()), Times.Once);
    }

    [Fact]
    public void SetFileMonitor_WhenDifferent_PersistsViaUpdate_AndKeepsAutoBackup()
    {
        var settings = CreateSettingsMock(new BackupOptions
        {
            Settings = new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true),
        });
        var service = new BackupService(settings.Object);

        service.SetFileMonitor(false);

        Assert.True(service.BackupSettings.IsAutoBackupEnabled);
        Assert.False(service.BackupSettings.IsFileMonitorEnabled);
        Assert.True(settings.Object.Value.Settings.IsAutoBackupEnabled);
        Assert.False(settings.Object.Value.Settings.IsFileMonitorEnabled);
        settings.Verify(s => s.Update(It.IsAny<Action<BackupOptions>>()), Times.Once);
    }
}
