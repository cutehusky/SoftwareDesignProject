namespace SoftwareDesignProject.Models.DTO;

public class UploadPluginRequest
{
    public IFormFile? ClientDLL { get; set; }
    public IFormFile? ServerDLL { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPremium { get; set; }
    public string? Category { get; set; }
}