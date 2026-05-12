using MyCarDb.Values;
using Xunit;

namespace MyCarDBTests;

public class IFileValueEqualityTests
{
    [Fact]
    public void ArrayStringValue_StructuralEqualsRegardlessOfArrayInstance()
    {
        var a = new ArrayStringValue { Name = "X", Value = ["a", "b"] };
        var b = new ArrayStringValue { Name = "X", Value = ["a", "b"] };

        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void ArrayStringValue_NotEqualWhenValuesDiffer()
    {
        var a = new ArrayStringValue { Name = "X", Value = ["a", "b"] };
        var b = new ArrayStringValue { Name = "X", Value = ["a", "c"] };

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void ArrayStringValue_NotEqualWhenNamesDiffer()
    {
        var a = new ArrayStringValue { Name = "X", Value = ["a"] };
        var b = new ArrayStringValue { Name = "Y", Value = ["a"] };

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void DictionaryStringStringValue_StructuralEqualsRegardlessOfDictInstance()
    {
        var a = new DictionaryStringStringValue
        {
            Name = "X",
            Value = new Dictionary<string, string> { ["k1"] = "v1", ["k2"] = "v2" },
        };
        var b = new DictionaryStringStringValue
        {
            Name = "X",
            Value = new Dictionary<string, string> { ["k1"] = "v1", ["k2"] = "v2" },
        };

        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void DictionaryStringStringValue_EqualIgnoresInsertionOrder()
    {
        var a = new DictionaryStringStringValue
        {
            Name = "X",
            Value = new Dictionary<string, string> { ["k1"] = "v1", ["k2"] = "v2" },
        };
        var b = new DictionaryStringStringValue
        {
            Name = "X",
            Value = new Dictionary<string, string> { ["k2"] = "v2", ["k1"] = "v1" },
        };

        Assert.Equal(a, b);
    }

    [Fact]
    public void DictionaryStringStringValue_NotEqualWhenValuesDiffer()
    {
        var a = new DictionaryStringStringValue
        {
            Name = "X",
            Value = new Dictionary<string, string> { ["k1"] = "v1" },
        };
        var b = new DictionaryStringStringValue
        {
            Name = "X",
            Value = new Dictionary<string, string> { ["k1"] = "v2" },
        };

        Assert.NotEqual(a, b);
    }
}
