using Contracts;
using DocumentEditor.Dto;
using DocumentEditor.Services;
using Moq;
using Xunit;

namespace DocumentEditor.Tests.Services;

public class PinnedEntitiesServiceTests
{
    private static Mock<IWritableOptions<PinnedEntities>> CreateSettingsMock(PinnedEntities? initial = null)
    {
        var current = initial ?? new PinnedEntities();
        var mock = new Mock<IWritableOptions<PinnedEntities>>();
        mock.SetupGet(s => s.Value).Returns(() => current);
        mock.Setup(s => s.Update(It.IsAny<Action<PinnedEntities>>()))
            .Callback<Action<PinnedEntities>>(action => action(current));
        return mock;
    }

    [Fact]
    public void Entities_InitialState_ReadsFromSettings()
    {
        var settings = CreateSettingsMock(new PinnedEntities { Entities = ["satsuma", "kilju"] });

        var service = new PinnedEntitiesService(settings.Object);

        Assert.Equal(["satsuma", "kilju"], service.Entities);
    }

    [Fact]
    public void Entities_DefaultEmpty()
    {
        var settings = CreateSettingsMock();

        var service = new PinnedEntitiesService(settings.Object);

        Assert.Empty(service.Entities);
    }

    [Fact]
    public void Add_AppendsAndPersists()
    {
        var settings = CreateSettingsMock();
        var service = new PinnedEntitiesService(settings.Object);

        service.Add("satsuma");

        Assert.Equal(["satsuma"], service.Entities);
        settings.Verify(s => s.Update(It.IsAny<Action<PinnedEntities>>()), Times.Once);
    }

    [Fact]
    public void Add_MultipleEntries_AllPersisted()
    {
        var settings = CreateSettingsMock();
        var service = new PinnedEntitiesService(settings.Object);

        service.Add("satsuma");
        service.Add("kilju");
        service.Add("spark_plug");

        Assert.Equal(["satsuma", "kilju", "spark_plug"], service.Entities);
        settings.Verify(s => s.Update(It.IsAny<Action<PinnedEntities>>()), Times.Exactly(3));
    }

    [Fact]
    public void Remove_ExistingEntry_RemovesAndPersists()
    {
        var settings = CreateSettingsMock(new PinnedEntities { Entities = ["satsuma", "kilju"] });
        var service = new PinnedEntitiesService(settings.Object);

        service.Remove("satsuma");

        Assert.Equal(["kilju"], service.Entities);
        settings.Verify(s => s.Update(It.IsAny<Action<PinnedEntities>>()), Times.Once);
    }

    [Fact]
    public void Remove_NonExistent_DoesNotThrow()
    {
        var settings = CreateSettingsMock(new PinnedEntities { Entities = ["kilju"] });
        var service = new PinnedEntitiesService(settings.Object);

        var ex = Record.Exception(() => service.Remove("nonexistent"));

        Assert.Null(ex);
        Assert.Equal(["kilju"], service.Entities);
    }

    [Fact]
    public void Entities_ReflectsLatestState_AfterMutations()
    {
        var settings = CreateSettingsMock();
        var service = new PinnedEntitiesService(settings.Object);

        service.Add("a");
        service.Add("b");
        service.Remove("a");

        Assert.Equal(["b"], service.Entities);
    }
}
