namespace CommonDTO;

public class NavItem
{
    public Guid? PluginId { get; init; }
    public required string Text { get; init; }
    public required string Href { get; init; }
    public required string Icon { get; init; }
    public required string Category { get; init; }

    public bool IsPremium { get; init; } = false;
}