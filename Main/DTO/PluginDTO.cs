namespace CommonDTO;


    
public class PluginDTO
{
    public Guid PluginId { get; init; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public string? Category { get; set; }

    public bool? IsEnabled { get; set; } = true;

    public bool? IsPremium { get; set; } = false;
}
