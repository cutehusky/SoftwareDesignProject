namespace SoftwareDesignProject.Models.DTO;

public class UploadPluginRequest
{
    public required IFormFile ClientDLL { get; set; } 
    public IFormFile? ServerDLL { get; set; } = null;
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsPremium { get; set; }
    public required string Category { get; set; }
    public string? Icon { get; set; }
}