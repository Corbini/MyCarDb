using Moq;
using MyCarDbView.Items;
using MyCarDbView.Services;
using MyCarDbView.ViewModels;
using MyCarDbWorld.Models;
using Xunit;

namespace MyCarDbViewTests.ViewModels;

public class EntityBrowserViewModelTests
{
    private sealed record TestEntity(string Id) : IEntity;

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

    [Fact]
    public void UpdateEntities_Null_ClearsCategories()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock().Object);

        vm.UpdateEntities(null);

        Assert.Empty(vm.Categories);
        Assert.True(vm.IsEmptyResults);
    }

    [Fact]
    public void UpdateEntities_BuildsCategoryGroupedByType()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock().Object);

        vm.UpdateEntities([new TestEntity("a"), new TestEntity("b")]);

        var category = Assert.Single(vm.Categories);
        Assert.Equal(nameof(TestEntity), category.Title);
        Assert.Equal(2, category.Items.Count);
    }

    [Fact]
    public void UpdateEntities_MarksPinned_FromService()
    {
        var pinned = CreatePinnedMock("a");
        var vm = new EntityBrowserViewModel(pinned.Object);

        vm.UpdateEntities([new TestEntity("a"), new TestEntity("b")]);

        var pinnedSection = vm.Categories.First();
        Assert.Equal("EntityBrowser_pinned", pinnedSection.Title);
        var pinnedItem = Assert.Single(pinnedSection.Items);
        Assert.Equal("a", pinnedItem.Entity?.Id);
        Assert.True(pinnedItem.IsPinned);
    }

    [Fact]
    public void UpdateEntities_NoPinned_SkipsPinnedSection()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock().Object);

        vm.UpdateEntities([new TestEntity("a")]);

        Assert.DoesNotContain(vm.Categories, c => c.Title == "EntityBrowser_pinned");
    }

    [Fact]
    public void TogglePin_NotPinned_CallsAdd_AndFlipsItem()
    {
        var pinned = CreatePinnedMock();
        var vm = new EntityBrowserViewModel(pinned.Object);
        vm.UpdateEntities([new TestEntity("a")]);
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
        var vm = new EntityBrowserViewModel(pinned.Object);
        vm.UpdateEntities([new TestEntity("a")]);
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
        var vm = new EntityBrowserViewModel(pinned.Object);
        var orphan = new EntityBrowserItem { Title = "x", Entity = null, Items = [] };

        vm.TogglePinCommand.Execute(orphan);

        pinned.Verify(s => s.Add(It.IsAny<string>()), Times.Never);
        pinned.Verify(s => s.Remove(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Rebuild_PinnedSection_AppearsFirst()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock("b").Object);

        vm.UpdateEntities([new TestEntity("a"), new TestEntity("b"), new TestEntity("c")]);

        Assert.Equal("EntityBrowser_pinned", vm.Categories[0].Title);
        Assert.Equal(nameof(TestEntity), vm.Categories[1].Title);
    }

    [Fact]
    public void SearchText_Filters_NonPinnedItems()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock().Object);
        vm.UpdateEntities([new TestEntity("satsuma"), new TestEntity("kilju"), new TestEntity("spark")]);

        vm.SearchText = "sa";

        var category = Assert.Single(vm.Categories);
        var item = Assert.Single(category.Items);
        Assert.Equal("satsuma", item.Entity?.Id);
    }

    [Fact]
    public void SearchText_PinnedItems_NotFiltered()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock("kilju").Object);
        vm.UpdateEntities([new TestEntity("satsuma"), new TestEntity("kilju")]);

        vm.SearchText = "satsuma";

        var pinnedSection = vm.Categories.First(c => c.Title == "EntityBrowser_pinned");
        Assert.Single(pinnedSection.Items);
        Assert.Equal("kilju", pinnedSection.Items[0].Entity?.Id);
    }

    [Fact]
    public void SearchText_NoMatches_IsEmptyResultsTrue()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock().Object);
        vm.UpdateEntities([new TestEntity("satsuma")]);

        vm.SearchText = "xxxx";

        Assert.True(vm.IsEmptyResults);
        Assert.Empty(vm.Categories);
    }

    [Fact]
    public void SearchText_CaseInsensitive()
    {
        var vm = new EntityBrowserViewModel(CreatePinnedMock().Object);
        vm.UpdateEntities([new TestEntity("Satsuma"), new TestEntity("KILJU")]);

        vm.SearchText = "satsuma";

        var category = Assert.Single(vm.Categories);
        Assert.Single(category.Items);
    }

    [Fact]
    public void UpdateEntities_AfterReload_PinnedStateRecovers()
    {
        var pinned = CreatePinnedMock("a");
        var vm = new EntityBrowserViewModel(pinned.Object);

        // simulate first load
        vm.UpdateEntities([new TestEntity("a"), new TestEntity("b")]);
        Assert.True(vm.Categories[0].Items[0].IsPinned);

        // simulate file reload — new entity instances same Ids
        vm.UpdateEntities([new TestEntity("a"), new TestEntity("c")]);

        var pinnedSection = vm.Categories.First(c => c.Title == "EntityBrowser_pinned");
        Assert.Single(pinnedSection.Items);
        Assert.Equal("a", pinnedSection.Items[0].Entity?.Id);
        Assert.True(pinnedSection.Items[0].IsPinned);
    }
}
