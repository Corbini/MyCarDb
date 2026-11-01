using System.Text.Json.Serialization;
using DocumentEditor.Dto;
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
[JsonSerializable(typeof(PinnedEntities))]
[JsonSerializable(typeof(BackupOptions))]
internal partial class AppJsonContext : JsonSerializerContext;
