namespace CommonDTO;

public class HomeItem
{
    public required Guid PluginId { get; init; }
    public required string Text { get; init; }
    public required string Description { get; init; }
    public required string Href { get; init; }
    public required string Icon { get; init; }

    public bool IsPremium { get; init; }
}