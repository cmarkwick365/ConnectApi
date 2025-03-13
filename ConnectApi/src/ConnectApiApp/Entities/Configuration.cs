

namespace ConnectApiApp.Entities;

public class Configuration
{
    public int IdConfig { get; set; }

    public required string ConfigName { get; set; }

    public required string Element { get; set; }

    public required string? Value { get; set; }
}