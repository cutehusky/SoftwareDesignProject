using System.ComponentModel.DataAnnotations;

namespace SoftwareDesignProject.Models.Entities;

public class Plugin
{
    public static readonly string DefaultCategory = "Uncategorized";
    
    [Key]
    public Guid PluginId { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = DefaultCategory;
    public bool IsEnabled { get; set; } = true;
    public bool IsPremium { get; set; }
}