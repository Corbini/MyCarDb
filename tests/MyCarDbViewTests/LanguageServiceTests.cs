using System.Globalization;
using Contracts;
using Moq;
using MyCarDbView.Dto;
using MyCarDbView.Models.AppState;
using MyCarDbView.Services;
using Xunit;

namespace MyCarDbViewTests.Services;

public class LanguageServiceTests : IDisposable
{
    private readonly CultureInfo _originalCulture;
    private readonly CultureInfo _originalUiCulture;

    public LanguageServiceTests()
    {
        _originalCulture = CultureInfo.CurrentCulture;
        _originalUiCulture = CultureInfo.CurrentUICulture;
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = _originalCulture;
        CultureInfo.CurrentUICulture = _originalUiCulture;
    }

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
    public void Constructor_ReadsLanguageFromSettings()
    {
        var settings = CreateSettingsMock(new AppSettings { Language = AppLanguage.Polish });

        var service = new LanguageService(settings.Object);

        Assert.Equal(AppLanguage.Polish, service.CurrentLanguage);
    }

    [Fact]
    public void SetLanguage_WhenSameValue_DoesNotCallUpdate()
    {
        var settings = CreateSettingsMock(new AppSettings { Language = AppLanguage.Polish });
        var service = new LanguageService(settings.Object);

        service.SetLanguage(AppLanguage.Polish);

        settings.Verify(s => s.Update(It.IsAny<Action<AppSettings>>()), Times.Never);
    }

    [Fact]
    public void SetLanguage_WhenDifferent_PersistsViaUpdate()
    {
        var settings = CreateSettingsMock(new AppSettings { Language = AppLanguage.System });
        var service = new LanguageService(settings.Object);

        service.SetLanguage(AppLanguage.Polish);

        Assert.Equal(AppLanguage.Polish, service.CurrentLanguage);
        settings.Verify(s => s.Update(It.IsAny<Action<AppSettings>>()), Times.Once);
        Assert.Equal(AppLanguage.Polish, settings.Object.Value.Language);
    }
}
