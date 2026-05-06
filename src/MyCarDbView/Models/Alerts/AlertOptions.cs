namespace MyCarDbView.Models.Alerts;

public sealed record AlertOptions
{
    public AlertSeverity Severity { get; init; } = AlertSeverity.Informational;

    public required string Title { get; init; }

    public string? Message { get; init; }
}
