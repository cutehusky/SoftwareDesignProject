using Microsoft.AspNetCore.Components.Forms;

namespace SoftwareDesignProject.Client.Models;

public class PluginUploadData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPremium { get; set; }
    public IBrowserFile ClientDLL { get; set; }
    public IBrowserFile ServerDLL { get; set; }
    public string Category { get; set; }
}