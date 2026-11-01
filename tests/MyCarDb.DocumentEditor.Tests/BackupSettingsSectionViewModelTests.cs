using DocumentEditor.Models.AppState;
using DocumentEditor.Services;
using DocumentEditor.ViewModels;
using Moq;
using Xunit;

namespace DocumentEditor.Tests.ViewModels;

public class BackupSettingsSectionViewModelTests
{
    private static Mock<IBackupService> CreateBackupMock(BackupSettings? initial = null)
    {
        var current = initial ?? new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true);
        var mock = new Mock<IBackupService>();
        mock.SetupGet(s => s.BackupSettings).Returns(() => current);
        mock.Setup(s => s.SetAutoBackup(It.IsAny<bool>()))
            .Callback<bool>(v => current = current with { IsAutoBackupEnabled = v });
        mock.Setup(s => s.SetFileMonitor(It.IsAny<bool>()))
            .Callback<bool>(v => current = current with { IsFileMonitorEnabled = v });
        return mock;
    }

    [Fact]
    public void IsAutoBackupEnabled_InitialValue_ReadsFromBackupService()
    {
        var backup = CreateBackupMock(new BackupSettings(IsAutoBackupEnabled: false, IsFileMonitorEnabled: true));
        var vm = new BackupSettingsSectionViewModel(backup.Object);

        Assert.False(vm.IsAutoBackupEnabled);
    }

    [Fact]
    public void IsAutoBackupEnabled_Setter_CallsBackupService_WhenValueDiffers()
    {
        var backup = CreateBackupMock(new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true));
        var vm = new BackupSettingsSectionViewModel(backup.Object);

        vm.IsAutoBackupEnabled = false;

        backup.Verify(s => s.SetAutoBackup(false), Times.Once);
    }

    [Fact]
    public void IsFileMonitorEnabled_InitialValue_ReadsFromBackupService()
    {
        var backup = CreateBackupMock(new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: false));
        var vm = new BackupSettingsSectionViewModel(backup.Object);

        Assert.False(vm.IsFileMonitorEnabled);
    }

    [Fact]
    public void IsFileMonitorEnabled_Setter_CallsBackupService_WhenValueDiffers()
    {
        var backup = CreateBackupMock(new BackupSettings(IsAutoBackupEnabled: true, IsFileMonitorEnabled: true));
        var vm = new BackupSettingsSectionViewModel(backup.Object);

        vm.IsFileMonitorEnabled = false;

        backup.Verify(s => s.SetFileMonitor(false), Times.Once);
    }
}
