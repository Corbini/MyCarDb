using MyCarDbView.Items;
using MyCarDbWorld.Models;
using Xunit;

namespace MyCarDbViewTests.Items;

public class DisplayEntityTests
{
    private sealed class FakeEntity(string id) : IEntity
    {
        public string Id { get; } = id;
    }

    [Theory]
    [InlineData("ax(item3)", "Entity_ax", " #3")]
    [InlineData("ax(item4)", "Entity_ax", " #4")]
    [InlineData("sandbag(item1)", "Entity_sandbag", " #1")]
    [InlineData("sandbag(item2)", "Entity_sandbag", " #2")]
    [InlineData("warningtrianglex(item1)", "Entity_warningtrianglex", " #1")]
    [InlineData("tv remote control(item2)", "Entity_tv_remote_control", " #2")]
    [InlineData("floor jack(itemx)", "Entity_floor_jack", null)]
    [InlineData("garbage barrel(itemx)", "Entity_garbage_barrel", null)]
    [InlineData("fireworks bag(itemx)", "Entity_fireworks_bag", null)]
    [InlineData("basketball(Clone)", "Entity_basketball", null)]
    [InlineData("ratchet set(Clone)", "Entity_ratchet_set", null)]
    [InlineData("fireworksbag0", "Entity_fireworksbag0", null)]
    public void Constructor_ParsesIdFromSaveFormat(
        string id,
        string expectedTitleKey,
        string? expectedSerial)
    {
        var display = new DisplayEntity(new FakeEntity(id));

        Assert.Equal(expectedTitleKey, display.TitleKey);
        Assert.Equal(expectedSerial, display.Serial);
    }

    [Fact]
    public void Constructor_ExposesSourceEntity()
    {
        IEntity source = new FakeEntity("ax(item3)");

        var display = new DisplayEntity(source);

        Assert.Same(source, display.Value);
    }
}
