using MyCarDbView.Localizations;
using MyCarDbWorld.Models;

namespace MyCarDbView.Items;

public sealed class EntityItem
{
    public required IEntity Value { get; init; }
    public required LocalizedString Title { get; init; }
    public required string? Serial { get; init; }

    internal static (string Key, string? Serial) ParseId(string id)
    {
        string[] parts = id.Split(["(", "item", ")"], StringSplitOptions.RemoveEmptyEntries);
        string name = parts[0].Replace(' ', '_');
        string? serial = parts.Length > 1 && int.TryParse(parts[1], out int n) ? $" #{n}" : null;

        if (serial is null)
        {
            int splitAt = name.Length;
            while (splitAt > 0 && char.IsDigit(name[splitAt - 1]))
                splitAt--;
            if (name.Length - splitAt >= 2 && splitAt > 0)
            {
                serial = $" #{int.Parse(name.AsSpan(splitAt))}";
                name = name[..splitAt];
            }
        }

        return ($"Entity_{name}", serial);
    }
}
