using System.Text.Json.Serialization;
using MyCarDbView.Models.AppState;

namespace MyCarDbView.Dto;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    Converters = new[]
    {
        typeof(JsonStringEnumConverter<AppLanguage>),
        typeof(JsonStringEnumConverter<AppTheme>),
    })]
[JsonSerializable(typeof(AppSettings))]
[JsonSerializable(typeof(RecentFiles))]
internal partial class AppJsonContext : JsonSerializerContext;
