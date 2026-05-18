using MyCarDbWorld.Models;

namespace MyCarDbView.Items;

public sealed class DisplayEntity
{
    public IEntity Value { get; }

    public DisplayEntity(IEntity value)
    {
        Value = value;

        string[] parts = Value.Id.Split(["(", "item", ")"], StringSplitOptions.RemoveEmptyEntries);
        string key = parts[0].Replace(' ', '_');

        TitleKey = $"Entity_{key}";
        Serial = parts.Length > 1 && int.TryParse(parts[1], out int n) ? $" #{n}" : null;
    }

    public string TitleKey { get; }

    public string? Serial { get; }
}
