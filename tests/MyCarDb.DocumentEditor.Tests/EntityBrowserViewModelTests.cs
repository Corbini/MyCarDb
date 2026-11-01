using Contracts;
using DocumentEditor.Items;
using DocumentEditor.Services;
using DocumentEditor.ViewModels;
using GameWorld.Models;
using Moq;
using Xunit;

namespace DocumentEditor.Tests.ViewModels;

public class EntityBrowserViewModelTests
{
    private sealed record TestEntity(string Id) : IEntity;

    private readonly Mock<ILocalizationSource> _localization = CreateLocalizationMock();

    private static EntityItem Display(string id)
    {
        var ls = new LocalizedString($"Entity_{id}");
        ls.Value = id;
        return new EntityItem
        {
            Value = new TestEntity(id),
            Title = ls,
            Serial = null,
        };
    }

    private EntityBrowserViewModel Create(IPinnedEntitiesService pinned) =>
        new(pinned, _localization.Object);

    private static Mock<IPinnedEntitiesService> CreatePinnedMock(params string[] initiallyPinned)
    {
        var current = new List<string>(initiallyPinned);
        var mock = new Mock<IPinnedEntitiesService>();
        mock.SetupGet(s => s.Entities).Returns(() => current);
        mock.Setup(s => s.Add(It.IsAny<string>()))
            .Callback<string>(id => current.Add(id));
        mock.Setup(s => s.Remove(It.IsAny<string>()))
            .Callback<string>(id => current.Remove(id));
        return mock;
    }

    private static Mock<ILocalizationSource> CreateLocalizationMock()
    {
        var mock = new Mock<ILocalizationSource>();
        mock.Setup(x => x.Get(It.IsAny<string>())).Returns<string>(EchoLocalizedString);
        mock.Setup(x => x.GetDb(It.IsAny<string>())).Returns<string>(EchoLocalizedString);
        return mock;
    }

    private static LocalizedString EchoLocalizedString(string key)
    {
        var ls = new LocalizedString(key);
        ls.Value = key;
        return ls;
    }

    [Fact]
    public void UpdateEntities_Null_ClearsCategories()
    {
        var vm = Create(CreatePinnedMock().Object);

        vm.UpdateEntities(null);

        Assert.Empty(vm.Categories);
        Assert.True(vm.IsEmptyResults);
    }

    [Fact]
    public void UpdateEntities_BuildsCategoryGroupedByType()
    {
        var vm = Create(CreatePinnedMock().Object);

        vm.UpdateEntities([Display("a"), Display("b")]);

        var category = Assert.Single(vm.Categories);
        Assert.Equal("Entity_Category_Others", category.Title?.Key);
        Assert.Equal(2, category.Items.Count);
    }

    [Fact]
    public void UpdateEntities_MarksPinned_FromService()
    {
        var pinned = CreatePinnedMock("a");
        var vm = Create(pinned.Object);

        vm.UpdateEntities([Display("a"), Display("b")]);

        var pinnedSection = vm.Categories.First();
        Assert.Equal("EntityBrowser_Section_Pinned", pinnedSection.Title?.Key);
        var pinnedItem = Assert.Single(pinnedSection.Items);
        Assert.Equal("a", pinnedItem.Entity?.Value.Id);
        Assert.True(pinnedItem.IsPinned);
    }

    [Fact]
    public void UpdateEntities_NoPinned_SkipsPinnedSection()
    {
        var vm = Create(CreatePinnedMock().Object);

        vm.UpdateEntities([Display("a")]);

        Assert.DoesNotContain(vm.Categories, c => c.Title?.Key == "EntityBrowser_Section_Pinned");
    }

    [Fact]
    public void TogglePin_NotPinned_CallsAdd_AndFlipsItem()
    {
        var pinned = CreatePinnedMock();
        var vm = Create(pinned.Object);
        vm.UpdateEntities([Display("a")]);
        var item = vm.Categories.SelectMany(c => c.Items).First();

        vm.TogglePinCommand.Execute(item);

        pinned.Verify(s => s.Add("a"), Times.Once);
        pinned.Verify(s => s.Remove(It.IsAny<string>()), Times.Never);
        Assert.True(item.IsPinned);
    }

    [Fact]
    public void TogglePin_Pinned_CallsRemove_AndFlipsItem()
    {
        var pinned = CreatePinnedMock("a");
        var vm = Create(pinned.Object);
        vm.UpdateEntities([Display("a")]);
        var item = vm.Categories.SelectMany(c => c.Items).First();

        vm.TogglePinCommand.Execute(item);

        pinned.Verify(s => s.Remove("a"), Times.Once);
        pinned.Verify(s => s.Add(It.IsAny<string>()), Times.Never);
        Assert.False(item.IsPinned);
    }

    [Fact]
    public void TogglePin_NullEntity_NoOp()
    {
        var pinned = CreatePinnedMock();
        var vm = Create(pinned.Object);
        var orphan = new EntityBrowserItem { Title = null, Items = [] };

        vm.TogglePinCommand.Execute(orphan);

        pinned.Verify(s => s.Add(It.IsAny<string>()), Times.Never);
        pinned.Verify(s => s.Remove(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Rebuild_PinnedSection_AppearsFirst()
    {
        var vm = Create(CreatePinnedMock("b").Object);

        vm.UpdateEntities([Display("a"), Display("b"), Display("c")]);

        Assert.Equal("EntityBrowser_Section_Pinned", vm.Categories[0].Title?.Key);
        Assert.Equal("Entity_Category_Others", vm.Categories[1].Title?.Key);
    }

    [Fact]
    public void SearchText_Filters_NonPinnedItems()
    {
        var vm = Create(CreatePinnedMock().Object);
        vm.UpdateEntities([Display("satsuma"), Display("kilju"), Display("spark")]);

        vm.SearchText = "sa";

        var category = Assert.Single(vm.Categories);
        var item = Assert.Single(category.Items);
        Assert.Equal("satsuma", item.Entity?.Value.Id);
    }

    [Fact]
    public void SearchText_PinnedItems_NotFiltered()
    {
        var vm = Create(CreatePinnedMock("kilju").Object);
        vm.UpdateEntities([Display("satsuma"), Display("kilju")]);

        vm.SearchText = "satsuma";

        var pinnedSection = vm.Categories.First(c => c.Title?.Key == "EntityBrowser_Section_Pinned");
        Assert.Single(pinnedSection.Items);
        Assert.Equal("kilju", pinnedSection.Items[0].Entity?.Value.Id);
    }

    [Fact]
    public void SearchText_NoMatches_IsEmptyResultsTrue()
    {
        var vm = Create(CreatePinnedMock().Object);
        vm.UpdateEntities([Display("satsuma")]);

        vm.SearchText = "xxxx";

        Assert.True(vm.IsEmptyResults);
        Assert.Empty(vm.Categories);
    }

    [Fact]
    public void SearchText_CaseInsensitive()
    {
        var vm = Create(CreatePinnedMock().Object);
        vm.UpdateEntities([Display("Satsuma"), Display("KILJU")]);

        vm.SearchText = "satsuma";

        var category = Assert.Single(vm.Categories);
        Assert.Single(category.Items);
    }

    [Fact]
    public void UpdateEntities_AfterReload_PinnedStateRecovers()
    {
        var pinned = CreatePinnedMock("a");
        var vm = Create(pinned.Object);

        vm.UpdateEntities([Display("a"), Display("b")]);
        Assert.True(vm.Categories[0].Items[0].IsPinned);

        vm.UpdateEntities([Display("a"), Display("c")]);

        var pinnedSection = vm.Categories.First(c => c.Title?.Key == "EntityBrowser_Section_Pinned");
        Assert.Single(pinnedSection.Items);
        Assert.Equal("a", pinnedSection.Items[0].Entity?.Value.Id);
        Assert.True(pinnedSection.Items[0].IsPinned);
    }
}
