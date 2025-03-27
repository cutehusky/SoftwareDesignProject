namespace CommonDTO;


    
public class PluginDTO
{
    public Guid PluginId { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public string? Category { get; set; }

    public bool? IsEnabled { get; set; } = true;

    public bool? IsPremium { get; set; } = false;
}
