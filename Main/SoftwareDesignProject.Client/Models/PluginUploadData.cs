using Microsoft.AspNetCore.Components.Forms;

namespace SoftwareDesignProject.Client.Models;

public class PluginUploadData
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsPremium { get; set; }
    public IBrowserFile ClientDLL { get; set; } = null!;
    public IBrowserFile? ServerDLL { get; set; }
    public required string Category { get; set; }
    public string? Icon { get; set; }
}