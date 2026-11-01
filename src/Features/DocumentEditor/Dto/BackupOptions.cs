using DocumentEditor.Models.AppState;

namespace DocumentEditor.Dto;

public sealed class BackupOptions
{
    public BackupSettings Settings { get; set; } = new(true, true);
}
