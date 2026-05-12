namespace MyCarDb.Values;

public record ColorValue : IFileValue
{
    public required string Name { get; init; }
    public required float Red { get; set; }
    public required float Green { get; set; }
    public required float Blue { get; set; }
    public required float Alpha { get; set; }
}
