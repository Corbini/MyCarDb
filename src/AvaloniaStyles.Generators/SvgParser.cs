using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AvaloniaStyles.Generators;

internal readonly record struct SvgFigureData(string PathData, uint? FillArgb);

internal static class SvgParser
{
    private static readonly CultureInfo _inv = CultureInfo.InvariantCulture;

    private static readonly HashSet<string> _skipElements = new()
    {
        "defs", "clipPath", "mask", "title", "desc", "metadata", "style"
    };

    public static (ViewBox ViewBox, IReadOnlyList<SvgFigureData> Figures) Parse(string svgContent)
    {
        var doc = XDocument.Parse(svgContent);
        var root = doc.Root ?? throw new InvalidOperationException("SVG has no root element");

        var viewBox = ParseViewBox(root.Attribute("viewBox")?.Value);
        var figures = new List<SvgFigureData>();
        uint? rootFill = ParseFill(root.Attribute("fill")?.Value);

        foreach (var child in root.Elements())
            Visit(child, rootFill, viewBox, figures);

        return (viewBox, figures);
    }

    private static void Visit(XElement el, uint? inheritedFill, ViewBox viewBox, List<SvgFigureData> figures)
    {
        if (_skipElements.Contains(el.Name.LocalName))
            return;

        uint? elFill = ParseFill(el.Attribute("fill")?.Value);
        uint? currentFill = elFill ?? inheritedFill;

        string part = ConvertShape(el, viewBox);
        if (!string.IsNullOrEmpty(part))
            figures.Add(new SvgFigureData(part, currentFill));

        foreach (var child in el.Elements())
            Visit(child, currentFill, viewBox, figures);
    }

    private static string ConvertShape(XElement el, ViewBox viewBox)
    {
        return el.Name.LocalName switch
        {
            "line" => ConvertLine(el),
            "rect" => ConvertRect(el, viewBox),
            "circle" => ConvertCircle(el),
            "path" => ConvertPath(el),
            "polyline" => ConvertPoints(el, closed: false),
            "polygon" => ConvertPoints(el, closed: true),
            _ => ""
        };
    }

    private static string ConvertLine(XElement el)
    {
        double x1 = D(el, "x1");
        double y1 = D(el, "y1");
        double x2 = D(el, "x2");
        double y2 = D(el, "y2");
        return $"M{F(x1)},{F(y1)} L{F(x2)},{F(y2)}";
    }

    private static string ConvertRect(XElement el, ViewBox viewBox)
    {
        double x = D(el, "x");
        double y = D(el, "y");
        double w = D(el, "width");
        double h = D(el, "height");
        double rx = D(el, "rx");
        string? fill = el.Attribute("fill")?.Value;

        // Skip viewport rect (covers whole canvas with fill="none" — Phosphor convention).
        if (x == 0 && y == 0 && w >= viewBox.Width && h >= viewBox.Height && fill == "none")
            return "";

        if (rx > 0)
        {
            var sb = new StringBuilder();
            sb.Append($"M{F(x + rx)},{F(y)} ");
            sb.Append($"H{F(x + w - rx)} ");
            sb.Append($"A{F(rx)},{F(rx)} 0 0 1 {F(x + w)},{F(y + rx)} ");
            sb.Append($"V{F(y + h - rx)} ");
            sb.Append($"A{F(rx)},{F(rx)} 0 0 1 {F(x + w - rx)},{F(y + h)} ");
            sb.Append($"H{F(x + rx)} ");
            sb.Append($"A{F(rx)},{F(rx)} 0 0 1 {F(x)},{F(y + h - rx)} ");
            sb.Append($"V{F(y + rx)} ");
            sb.Append($"A{F(rx)},{F(rx)} 0 0 1 {F(x + rx)},{F(y)} Z");
            return sb.ToString();
        }

        return $"M{F(x)},{F(y)} H{F(x + w)} V{F(y + h)} H{F(x)} Z";
    }

    private static string ConvertCircle(XElement el)
    {
        double cx = D(el, "cx");
        double cy = D(el, "cy");
        double r = D(el, "r");
        return $"M{F(cx - r)},{F(cy)} A{F(r)},{F(r)} 0 1 0 {F(cx + r)},{F(cy)} A{F(r)},{F(r)} 0 1 0 {F(cx - r)},{F(cy)} Z";
    }

    private static string ConvertPath(XElement el)
    {
        return el.Attribute("d")?.Value?.Trim() ?? "";
    }

    private static string ConvertPoints(XElement el, bool closed)
    {
        string raw = el.Attribute("points")?.Value ?? "";
        double[] coords = raw
            .Replace(',', ' ')
            .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => double.Parse(s, _inv))
            .ToArray();

        if (coords.Length < 4 || coords.Length % 2 != 0)
            return "";

        var sb = new StringBuilder();
        sb.Append($"M{F(coords[0])},{F(coords[1])}");
        for (int i = 2; i < coords.Length; i += 2)
            sb.Append($" L{F(coords[i])},{F(coords[i + 1])}");
        if (closed)
            sb.Append(" Z");
        return sb.ToString();
    }

    private static uint? ParseFill(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        value = value!.Trim();
        if (value.Equals("none", StringComparison.OrdinalIgnoreCase))
            return null;

        if (value.StartsWith("#"))
        {
            string hex = value.Substring(1);
            if (hex.Length == 3)
                hex = $"{hex[0]}{hex[0]}{hex[1]}{hex[1]}{hex[2]}{hex[2]}";
            if (hex.Length == 6 && uint.TryParse(hex, NumberStyles.HexNumber, _inv, out uint rgb))
                return 0xFF000000u | rgb;
            if (hex.Length == 8 && uint.TryParse(hex, NumberStyles.HexNumber, _inv, out uint argb))
                return argb;
        }

        if (value.Equals("white", StringComparison.OrdinalIgnoreCase))
            return 0xFFFFFFFFu;
        if (value.Equals("black", StringComparison.OrdinalIgnoreCase))
            return 0xFF000000u;

        return null;
    }

    private static double D(XElement el, string attr)
    {
        string? v = el.Attribute(attr)?.Value;
        return string.IsNullOrEmpty(v) ? 0 : double.Parse(v!, _inv);
    }

    private static string F(double v) => v.ToString("0.###", _inv);

    private static ViewBox ParseViewBox(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return new ViewBox(0, 0, 256, 256);
        string[] parts = raw!.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 4)
            return new ViewBox(0, 0, 256, 256);
        return new ViewBox(
            double.Parse(parts[0], _inv),
            double.Parse(parts[1], _inv),
            double.Parse(parts[2], _inv),
            double.Parse(parts[3], _inv));
    }

    public readonly struct ViewBox
    {
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public ViewBox(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
