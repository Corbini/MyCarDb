using Moq;
using MyCarDbView.Models.AppState;
using MyCarDbView.Services;
using MyCarDbView.ViewModels;
using Xunit;

namespace MyCarDbViewTests.ViewModels;

public class SettingsViewModelTests
{
    private static Mock<ILanguageService> CreateLanguageMock(AppLanguage initial = AppLanguage.System)
    {
        var current = initial;
        var mock = new Mock<ILanguageService>();
        mock.SetupGet(s => s.CurrentLanguage).Returns(() => current);
        mock.Setup(s => s.SetLanguage(It.IsAny<AppLanguage>()))
            .Callback<AppLanguage>(l => current = l);
        return mock;
    }

    private static Mock<IThemeService> CreateThemeMock(AppTheme initial = AppTheme.System)
    {
        var current = initial;
        var mock = new Mock<IThemeService>();
        mock.SetupGet(s => s.CurrentTheme).Returns(() => current);
        mock.Setup(s => s.SetTheme(It.IsAny<AppTheme>()))
            .Callback<AppTheme>(t => current = t);
        return mock;
    }

    private static SettingsViewModel BuildVm(
        Mock<ILanguageService>? language = null,
        Mock<IThemeService>? theme = null) =>
        new(
            (language ?? CreateLanguageMock()).Object,
            (theme ?? CreateThemeMock()).Object,
            licenseService: Mock.Of<ILicenseService>(),
            dialogService: Mock.Of<IDialogService>());

    [Fact]
    public void DefaultCategory_IsGeneral_AndOnlyIsGeneralSelectedFlagIsTrue()
    {
        var vm = BuildVm();

        Assert.Equal(SettingsCategory.General, vm.SelectedCategory);
        Assert.True(vm.IsGeneralSelected);
        Assert.False(vm.IsShortcutsSelected);
        Assert.False(vm.IsAboutSelected);
        Assert.False(vm.IsLicensesSelected);
    }

    [Fact]
    public void SelectCategoryCommand_ChangesCategory_AndRaisesINPCForAllFlags()
    {
        var vm = BuildVm();
        var raised = new List<string?>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        vm.SelectCategoryCommand.Execute(SettingsCategory.Shortcuts);

        Assert.Equal(SettingsCategory.Shortcuts, vm.SelectedCategory);
        Assert.True(vm.IsShortcutsSelected);
        Assert.False(vm.IsGeneralSelected);
        Assert.Contains(nameof(SettingsViewModel.IsGeneralSelected), raised);
        Assert.Contains(nameof(SettingsViewModel.IsShortcutsSelected), raised);
        Assert.Contains(nameof(SettingsViewModel.IsAboutSelected), raised);
        Assert.Contains(nameof(SettingsViewModel.IsLicensesSelected), raised);
    }

    [Fact]
    public void SelectedLanguage_Setter_IsNoOp_WhenValueEqualsCurrent()
    {
        var lang = CreateLanguageMock(AppLanguage.System);
        var vm = BuildVm(language: lang);

        vm.SelectedLanguage = AppLanguage.System;

        lang.Verify(s => s.SetLanguage(It.IsAny<AppLanguage>()), Times.Never);
    }

    [Fact]
    public void SelectedLanguage_Setter_CallsService_WhenValueDiffers()
    {
        var lang = CreateLanguageMock(AppLanguage.System);
        var vm = BuildVm(language: lang);

        vm.SelectedLanguage = AppLanguage.Polish;

        lang.Verify(s => s.SetLanguage(AppLanguage.Polish), Times.Once);
    }

    [Fact]
    public void SelectThemeCommand_CallsThemeService_AndUpdatesIsXThemeFlags()
    {
        var theme = CreateThemeMock(AppTheme.System);
        var vm = BuildVm(theme: theme);
        var raised = new List<string?>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        vm.SelectThemeCommand.Execute(AppTheme.Dark);

        theme.Verify(s => s.SetTheme(AppTheme.Dark), Times.Once);
        Assert.True(vm.IsDarkTheme);
        Assert.False(vm.IsSystemTheme);
        Assert.False(vm.IsLightTheme);
        Assert.Contains(nameof(SettingsViewModel.IsSystemTheme), raised);
        Assert.Contains(nameof(SettingsViewModel.IsLightTheme), raised);
        Assert.Contains(nameof(SettingsViewModel.IsDarkTheme), raised);
    }

    [Fact]
    public void SelectedTheme_Setter_IsNoOp_WhenValueEqualsCurrent()
    {
        var theme = CreateThemeMock(AppTheme.System);
        var vm = BuildVm(theme: theme);

        vm.SelectedTheme = AppTheme.System;

        theme.Verify(s => s.SetTheme(It.IsAny<AppTheme>()), Times.Never);
    }
}
