namespace SoftwareDesignProject.Client.Models;

public class PluginItem
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public bool Enabled { get; set; }
    
    public bool Premium { get; set; }
}