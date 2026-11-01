using AvaloniaStyles.Generators;
using Xunit;

namespace AvaloniaStyles.Generators.Tests;

public class SvgParserTests
{
    [Fact]
    public void Parse_SimplePath_ReturnsOneFigureWithNullFill()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 256 256">
                <path d="M10,10 L20,20"/>
            </svg>
            """;

        var (viewBox, figures) = SvgParser.Parse(svg);

        Assert.Equal(0, viewBox.X);
        Assert.Equal(0, viewBox.Y);
        Assert.Equal(256, viewBox.Width);
        Assert.Equal(256, viewBox.Height);
        Assert.Single(figures);
        Assert.Equal("M10,10 L20,20", figures[0].PathData);
        Assert.Null(figures[0].FillArgb);
    }

    [Fact]
    public void Parse_PathWithRrggbbFill_ReturnsFullAlpha()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <path d="M0,0" fill="#FF8800"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Single(figures);
        Assert.Equal(0xFFFF8800u, figures[0].FillArgb);
    }

    [Fact]
    public void Parse_ShortHexFill_ExpandsToFullArgb()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <path d="M0,0" fill="#F80"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Equal(0xFFFF8800u, figures[0].FillArgb);
    }

    [Fact]
    public void Parse_HexRrggbbaaFill_PreservesAlphaByte()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <path d="M0,0" fill="#80FF8800"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Equal(0x80FF8800u, figures[0].FillArgb);
    }

    [Fact]
    public void Parse_FillNone_ReturnsNullFill()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <path d="M0,0" fill="none"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Single(figures);
        Assert.Null(figures[0].FillArgb);
    }

    [Theory]
    [InlineData("white", 0xFFFFFFFFu)]
    [InlineData("black", 0xFF000000u)]
    public void Parse_NamedColor_ResolvesToArgb(string name, uint expected)
    {
        string svg = $"""
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <path d="M0,0" fill="{name}"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Equal(expected, figures[0].FillArgb);
    }

    [Fact]
    public void Parse_GroupFill_IsInheritedByChildPath()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <g fill="#AA0000">
                    <path d="M0,0"/>
                    <path d="M10,10" fill="#00FF00"/>
                </g>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Equal(2, figures.Count);
        Assert.Equal(0xFFAA0000u, figures[0].FillArgb);
        Assert.Equal(0xFF00FF00u, figures[1].FillArgb);
    }

    [Fact]
    public void Parse_DefsSubtree_IsIgnored()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <defs>
                    <path d="M99,99"/>
                </defs>
                <path d="M10,10"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Single(figures);
        Assert.Equal("M10,10", figures[0].PathData);
    }

    [Fact]
    public void Parse_ClipPathSubtree_IsIgnored()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <clipPath id="x">
                    <rect width="100" height="100" fill="white"/>
                </clipPath>
                <path d="M5,5"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Single(figures);
        Assert.Equal("M5,5", figures[0].PathData);
    }

    [Fact]
    public void Parse_MissingViewBox_FallsBackToDefault256()
    {
        const string svg = """<svg xmlns="http://www.w3.org/2000/svg"><path d="M0,0"/></svg>""";

        var (viewBox, _) = SvgParser.Parse(svg);

        Assert.Equal(0, viewBox.X);
        Assert.Equal(0, viewBox.Y);
        Assert.Equal(256, viewBox.Width);
        Assert.Equal(256, viewBox.Height);
    }

    [Fact]
    public void Parse_PhosphorViewportRect_IsSkipped()
    {
        // Phosphor convention: full-bounds rect with fill="none" is a viewport marker, not content.
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 256 256">
                <rect width="256" height="256" fill="none"/>
                <path d="M10,10"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Single(figures);
        Assert.Equal("M10,10", figures[0].PathData);
    }

    [Fact]
    public void Parse_RectWithDimensions_ConvertsToPath()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <rect x="10" y="20" width="30" height="40" fill="#FF0000"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Single(figures);
        Assert.Equal("M10,20 H40 V60 H10 Z", figures[0].PathData);
        Assert.Equal(0xFFFF0000u, figures[0].FillArgb);
    }

    [Fact]
    public void Parse_MultiplePathsWithDifferentFills_PreservesOrder()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
                <path d="M1,1" fill="#FF0000"/>
                <path d="M2,2" fill="#00FF00"/>
                <path d="M3,3" fill="#0000FF"/>
            </svg>
            """;

        var (_, figures) = SvgParser.Parse(svg);

        Assert.Equal(3, figures.Count);
        Assert.Equal("M1,1", figures[0].PathData);
        Assert.Equal(0xFFFF0000u, figures[0].FillArgb);
        Assert.Equal("M2,2", figures[1].PathData);
        Assert.Equal(0xFF00FF00u, figures[1].FillArgb);
        Assert.Equal("M3,3", figures[2].PathData);
        Assert.Equal(0xFF0000FFu, figures[2].FillArgb);
    }

    [Fact]
    public void Parse_ViewBoxWithNonZeroOrigin_ParsesAllFourValues()
    {
        const string svg = """
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="-10 -20 100 200">
                <path d="M0,0"/>
            </svg>
            """;

        var (viewBox, _) = SvgParser.Parse(svg);

        Assert.Equal(-10, viewBox.X);
        Assert.Equal(-20, viewBox.Y);
        Assert.Equal(100, viewBox.Width);
        Assert.Equal(200, viewBox.Height);
    }
}
