namespace SoftwareDesignProject.Models.DTO;

public class UpgradePluginRequest
{
    public required Guid PluginId { get; set; }
    public required IFormFile? ClientDLL { get; set; } 
    public IFormFile? ServerDLL { get; set; } = null;
}