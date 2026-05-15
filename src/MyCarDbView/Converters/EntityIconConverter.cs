using Avalonia.Data.Converters;
using MyCarDbWorld.Models;

namespace MyCarDbView.Converters;

public static class EntityIconConverter
{
    public static readonly IValueConverter Instance =
        new FuncValueConverter<IEntity?, string?>(GetIconSymbol);

    private static string? GetIconSymbol(IEntity? entity) => entity switch
    {
        PropertyEntity => "backpack",
        _ => null,
    };
}
