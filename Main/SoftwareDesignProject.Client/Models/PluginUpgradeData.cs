using Microsoft.AspNetCore.Components.Forms;

namespace SoftwareDesignProject.Client.Models;

public class PluginUpgradeData
{
    public Guid PluginId { get; set; }
    public IBrowserFile? ClientDLL { get; set; }
    public IBrowserFile? ServerDLL { get; set; }
}