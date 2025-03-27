using System.ComponentModel.DataAnnotations;

namespace SoftwareDesignProject.Models.Entities;

public class Plugin
{
    [Key]
    public Guid PluginId { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public bool IsPremium { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
}