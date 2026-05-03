using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AvaloniaStyles.Generators;

internal static class SvgParser
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    public static (ViewBox ViewBox, string PathData) Parse(string svgContent)
    {
        var doc = XDocument.Parse(svgContent);
        var root = doc.Root ?? throw new InvalidOperationException("SVG has no root element");

        var viewBox = ParseViewBox(root.Attribute("viewBox")?.Value);
        var parts = new List<string>();

        foreach (var element in root.Descendants())
        {
            string part = ConvertShape(element, viewBox);
            if (!string.IsNullOrEmpty(part))
                parts.Add(part);
        }

        return (viewBox, string.Join(" ", parts));
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
            // Rounded rect — 4 corners as arcs.
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
        // Two semicircular arcs to close the loop.
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
            .Select(s => double.Parse(s, Inv))
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

    private static double D(XElement el, string attr)
    {
        string? v = el.Attribute(attr)?.Value;
        return string.IsNullOrEmpty(v) ? 0 : double.Parse(v!, Inv);
    }

    private static string F(double v) => v.ToString("0.###", Inv);

    private static ViewBox ParseViewBox(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return new ViewBox(0, 0, 256, 256);
        string[] parts = raw!.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 4)
            return new ViewBox(0, 0, 256, 256);
        return new ViewBox(
            double.Parse(parts[0], Inv),
            double.Parse(parts[1], Inv),
            double.Parse(parts[2], Inv),
            double.Parse(parts[3], Inv));
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
