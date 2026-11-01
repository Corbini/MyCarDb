using MyCarDbView.Models.AppState;

namespace MyCarDbView.Dto;

public class AppSettings
{
    public AppLanguage Language { get; set; } = AppLanguage.System;
    public AppTheme Theme { get; set; } = AppTheme.System;
}
